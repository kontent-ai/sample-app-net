using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Globalization;
using System.Linq;

namespace AspNetCore.Mvc.Routing.Localization.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizedRouting(this IServiceCollection services, CultureInfo[] supportedCultures)
        {
            if(supportedCultures != null && supportedCultures.Any())
            {
                services.Configure<RequestLocalizationOptions>(options =>
                {
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
            }

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IControllerActionDescriptorProvider, ControllerActionDescriptorProvider>();
            services.TryAddSingleton<ILocalizedRoutingDynamicRouteValueResolver, LocalizedRoutingDynamicRouteValueResolver>();
            services.TryAddSingleton<ILocalizedRoutingProvider, LocalizedRouteProvider>();
            return services;
        }
    }
}
