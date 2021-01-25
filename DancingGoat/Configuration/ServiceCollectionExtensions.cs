using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DancingGoat.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(this IServiceCollection services, IConfigurationRoot root, IConfigurationSection section, string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var environment = provider.GetService<IWebHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new WritableOptions<T>(environment, root, options, section.Key, file);
            });
        }

        public static void ConfigureRequestLocalization(this IServiceCollection services, string defaultCulture, params string[] otherSupportedCultures)
        {
            var defaultCultureInfo = new CultureInfo(defaultCulture);
            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new()
                {
                    defaultCultureInfo
                };

                supportedCultures.AddRange(otherSupportedCultures.Select(culture => new CultureInfo(culture)));

                options.DefaultRequestCulture = new RequestCulture(culture: defaultCultureInfo, uiCulture: defaultCultureInfo);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.AddInitialRequestCultureProvider(new RouteDataRequestCultureProvider()
                {
                    RouteDataStringKey = "culture"
                });
            });
        }
    }
}
