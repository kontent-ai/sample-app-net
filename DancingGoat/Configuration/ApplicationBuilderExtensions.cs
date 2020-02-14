using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Rewrite;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DancingGoat.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRewriter(this IApplicationBuilder app, string path) 
        {
            using (var urlRewriteStreamReader = File.OpenText(path))
            {
                var options = new RewriteOptions().AddIISUrlRewrite(urlRewriteStreamReader);
                app.UseRewriter(options);
            }
        }

        public static void UseRequestLocalization(this IApplicationBuilder app, string defaultCulture, params string[] otherSupportedCultures)
        {
            var defaultCultureInfo = new CultureInfo(defaultCulture);
            List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                defaultCultureInfo
            };
            
            supportedCultures.AddRange(otherSupportedCultures.Select(culture => new CultureInfo(culture)));

            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCultureInfo),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            requestLocalizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider()
            {
                RouteDataStringKey = "culture",
                Options = requestLocalizationOptions
            });

            app.UseRequestLocalization(requestLocalizationOptions);
        }
    }
}
