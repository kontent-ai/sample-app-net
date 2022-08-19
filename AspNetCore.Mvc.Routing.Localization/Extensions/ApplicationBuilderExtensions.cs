using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Collections.Generic;
using System.Globalization;

namespace AspNetCore.Mvc.Routing.Localization.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLocalizedRouting(this IApplicationBuilder app, string defaultRequestCulture, CultureInfo[] supportedCultures)
        {
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultRequestCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new RouteDataRequestCultureProvider() { RouteDataStringKey = "culture" }
            };

            app.UseRequestLocalization(options);

            return app;
        }
    }
}
