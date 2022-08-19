using AspNetCore.Mvc.Routing.Localization.Attributes;
using AspNetCore.Mvc.Routing.Localization.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Routing.Localization
{
    /// <summary>
    /// Loads and provides localized routes. 
    /// You can inherit it and override the source of localized routes.
    /// </summary>
    public class LocalizedRouteProvider : ILocalizedRoutingProvider
    {
        private IEnumerable<LocalizedRoute> _routes = Enumerable.Empty<LocalizedRoute>();
        private bool _routesLoaded = false;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private readonly IControllerActionDescriptorProvider _controllerActionDescriptorProvider;
        private readonly IList<CultureInfo> _supportedCultures;

        IEnumerable<LocalizedRoute> ILocalizedRoutingProvider.Routes => _routes;

        public LocalizedRouteProvider(IControllerActionDescriptorProvider controllerActionDescriptorProvider,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _controllerActionDescriptorProvider = controllerActionDescriptorProvider;
            _supportedCultures = localizationOptions.Value.SupportedCultures;
        }

        /// <summary>
        /// Provides a route - depends on the direction.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="controler"></param>
        /// <param name="action"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public async Task<RouteInformation> ProvideRouteAsync(string culture, string controler, string action, LocalizationDirection direction)
        {
            if (!_routesLoaded && !_routes.Any())
            {
                await _semaphore.WaitAsync();
                try
                {
                    _routes = await GetRoutesAsync();
                    _routesLoaded = true;
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return GetLocalizedRoute(culture, controler, action, direction);
        }

        /// <summary>
        /// Creates localized routes 
        /// You can override this method to change the source of localized routes - DB, File,...
        /// </summary>
        /// <returns>Localized routes</returns>
        protected virtual async Task<IEnumerable<LocalizedRoute>> GetRoutesAsync()
        {
            /* 
               
                LocalizedRouteController -  LocalizedRouteAction
                LocalizedRouteController -  RouteAction
                LocalizedRouteController -  OriginalAction    
                RouteController - LocalizedRouteAction 
                OriginalController - LocalizedRouteAction  
            
             */

            var localizedRoutes = new List<LocalizedRoute>();

            foreach (var routeDescriptor in _controllerActionDescriptorProvider.Get())
            {
                routeDescriptor.RouteValues.TryGetValue("controller", out var controller);
                routeDescriptor.RouteValues.TryGetValue("action", out var action);

                var controllerLocalizedRouteAttributes = GetControllersAttribute<LocalizedRouteAttribute>(routeDescriptor);
                var controllerRouteAttributes = GetControllersAttribute<RouteAttribute>(routeDescriptor);
                var actionLocalizedRouteAttributes = GetMethodsAttribute<LocalizedRouteAttribute>(routeDescriptor);
                var actionRouteAttributes = GetMethodsAttribute<RouteAttribute>(routeDescriptor);

                foreach (var controllerLocalizedRouteAttribute in controllerLocalizedRouteAttributes)
                {
                    var actionLocalizedRouteAttribute = actionLocalizedRouteAttributes
                       .FirstOrDefault(s => s.Culture == controllerLocalizedRouteAttribute.Culture);

                    if (actionLocalizedRouteAttribute != null)
                    {
                        AddLocalizedRoute(controllerLocalizedRouteAttribute.Culture, controllerLocalizedRouteAttribute.Template, actionLocalizedRouteAttribute.Template);
                    }
                    else if (actionRouteAttributes.Any())
                    {
                        foreach (var actionRouteAttribute in actionRouteAttributes)
                        {
                            AddLocalizedRoute(controllerLocalizedRouteAttribute.Culture, controllerLocalizedRouteAttribute.Template, actionRouteAttribute.Template);
                        }
                    }
                    else
                    {
                        AddLocalizedRoute(controllerLocalizedRouteAttribute.Culture, controllerLocalizedRouteAttribute.Template, action);
                    }
                }

                foreach (var controllerRouteAttribute in controllerRouteAttributes)
                {
                    foreach (var actionLocalizedRouteAttribute in actionLocalizedRouteAttributes)
                    {
                        AddLocalizedRoute(actionLocalizedRouteAttribute.Culture, controllerRouteAttribute.Template, actionLocalizedRouteAttribute.Template);
                    }

                    foreach (var actionRouteAttribute in actionRouteAttributes)
                    {
                        foreach (var culture in _supportedCultures)
                        {
                            AddLocalizedRoute(culture.Name, controllerRouteAttribute.Template, actionRouteAttribute.Template);
                        }
                    }

                    if (!actionRouteAttributes.Any() && !actionLocalizedRouteAttributes.Any())
                    {
                        foreach (var culture in _supportedCultures)
                        {
                            AddLocalizedRoute(culture.Name, controllerRouteAttribute.Template, action);
                        }
                    }
                }

                if (!controllerLocalizedRouteAttributes.Any() && !controllerRouteAttributes.Any())
                {
                    foreach (var actionLocalizedRouteAttribute in actionLocalizedRouteAttributes)
                    {
                        AddLocalizedRoute(actionLocalizedRouteAttribute.Culture, controller, actionLocalizedRouteAttribute.Template);
                    }

                    foreach (var actionRouteAttribute in actionRouteAttributes)
                    {
                        foreach (var culture in _supportedCultures)
                        {
                            AddLocalizedRoute(culture.Name, controller, actionRouteAttribute.Template);
                        }
                    }

                    if (!actionLocalizedRouteAttributes.Any() && !actionRouteAttributes.Any())
                    {
                        foreach (var culture in _supportedCultures)
                        {
                            AddLocalizedRoute(culture.Name, controller, action);
                        }
                    }
                }

                void AddLocalizedRoute(string cultureName, string controllerName, string actionName)
                {
                    localizedRoutes.Add(LocalizedRoute.Create(cultureName,
                                               RouteInformation.Create(controller, action),
                                               RouteInformation.Create(controllerName, actionName)));
                }
            }

            return localizedRoutes;
        }

        private RouteInformation GetLocalizedRoute(string culture, string controller, string action, LocalizationDirection direction)
        {
            Func<string, LocalizedRoute> translated = (currentCulture) =>
                _routes
                    .FirstOrDefault(s =>
                        s.Culture.Equals(currentCulture, StringComparison.OrdinalIgnoreCase) &&
                        s.Translated.Action.Equals(action, StringComparison.OrdinalIgnoreCase) &&
                        s.Translated.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase));

            Func<string, LocalizedRoute> original = (currentCulture) =>
                _routes
                    .FirstOrDefault(s =>
                        s.Culture.Equals(currentCulture, StringComparison.OrdinalIgnoreCase) &&
                        s.Original.Action.Equals(action, StringComparison.OrdinalIgnoreCase) &&
                        s.Original.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase));

            return direction == LocalizationDirection.TranslatedToOriginal
                          ? translated(culture).Original
                          : original(culture).Translated;
        }

        private static IEnumerable<T> GetMethodsAttribute<T>(ControllerActionDescriptor routeDescriptor) where T : class
        {
            return routeDescriptor.MethodInfo
                .GetCustomAttributes(typeof(T), true)
                .Select(s => s as T)
                .Distinct();
        }

        private static IEnumerable<T> GetControllersAttribute<T>(ControllerActionDescriptor routeDescriptor) where T : class
        {
            return routeDescriptor.ControllerTypeInfo
                .GetCustomAttributes(typeof(T), true)
                .Select(s => s as T)
                .Distinct();
        }
    }
}



