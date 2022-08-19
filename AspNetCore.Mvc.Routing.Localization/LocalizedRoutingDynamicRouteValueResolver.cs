using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Routing.Localization
{
    internal class LocalizedRoutingDynamicRouteValueResolver : ILocalizedRoutingDynamicRouteValueResolver
    {
        private readonly ILocalizedRoutingProvider _localizedRoutingProvider;

        public LocalizedRoutingDynamicRouteValueResolver(ILocalizedRoutingProvider localizedRoutingProvider)
        {
            _localizedRoutingProvider = localizedRoutingProvider;
        }

        public async Task<RouteValueDictionary> ResolveAsync(RouteValueDictionary values)
        {
            if (!values.TryGetValue("culture", out var culture) ||
                !values.TryGetValue("controller", out var controller) ||
                !values.TryGetValue("action", out var action)) return values;

            var routeInformationMetadata = await _localizedRoutingProvider.ProvideRouteAsync((string)culture, (string)controller, (string)action, LocalizationDirection.TranslatedToOriginal);

            values["controller"] = routeInformationMetadata.Controller;
            values["action"] = routeInformationMetadata.Action;

            return values;
        }
    }
}
