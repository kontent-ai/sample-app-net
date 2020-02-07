using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Areas.Admin.Infrastructure;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using Kentico.AspNetCore.LocalizedRouting.Extensions;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DancingGoat
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Enable configuration services
            services.AddOptions();

            //Enable Delivery Client
            services.AddSingleton<ITypeProvider, CustomTypeProvider>();
            services.AddSingleton<IContentLinkUrlResolver, CustomContentLinkUrlResolver>();
            services.AddDeliveryClient(Configuration);

            // ConfigurationManagerProvider is here now
            services.Configure<AppSettings>(Configuration.GetSection("AppConfiguration"));
            services.Configure<DeliveryOptions>(Configuration.GetSection(nameof(DeliveryOptions)));
            services.AddScoped<IAppSettingProvider, AppSettingProvider>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<ISelfConfigManager, SelfConfigManager>();
            services.AddSingleton<CustomLocalizedRoutingTranslationTransformer>();
            services.AddControllersWithViews();
            services.AddLocalizedRouting();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            using (var urlRewriteStreamReader = File.OpenText("UrlRewrite.xml"))
            {
                var options = new RewriteOptions().AddIISUrlRewrite(urlRewriteStreamReader);
                app.UseRewriter(options);
            }

            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("es-ES"),
            };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };

            requestLocalizationOptions.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider()
            {
                RouteDataStringKey = "culture",
                Options = requestLocalizationOptions
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseRequestLocalization(requestLocalizationOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicControllerRoute<CustomLocalizedRoutingTranslationTransformer>("{culture}/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute("default", "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            
        }
    }
}
