using System.Globalization;
using System.IO;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Areas.Admin.Infrastructure;
using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DancingGoat
{
    public class Startup
    {

        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange:true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            // enable configuration services
            services.AddOptions();
            // ConfigurationManagerProvider is here now
            services.Configure<AppSettings>(Configuration.GetSection("AppConfiguration"));
            services.Configure<DeliveryOptions>(options => options = new DeliveryOptions());
            services.AddScoped<IAppSettingProvider, AppSettingProvider>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("en-GB"),
                    new CultureInfo("de-DE")
                };
                options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting 
                // numbers, dates, etc.

                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, 
                // i.e. we have localized resources for.

                options.SupportedUICultures = supportedCultures;
            });
            services.AddDeliveryClient(new DeliveryOptions());
            services.AddScoped<ISelfConfigManager,SelfConfigManager>();
            services.AddRouting();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // see if we need an extension method or not for this
            using(var urlRewriteStreamReader = File.OpenText("UrlRewrite.xml"))
            {
                var options = new RewriteOptions().AddIISUrlRewrite(urlRewriteStreamReader);
                app.UseRewriter(options);
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc(routes=>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Replacing Areas/Admin/AdminAreaRegistration.cs
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
