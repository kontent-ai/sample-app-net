using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DancingGoat.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureWritable<T>(this IServiceCollection services, IConfigurationSection section, string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var environment = provider.GetService<IWebHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                var root = provider.GetService<IConfigurationRoot>();
                return new WritableOptions<T>(environment, root, options, section.Key, file);
            });
        }
    }
}
