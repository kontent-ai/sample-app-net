using DancingGoat.Configuration;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using DancingGoat.Repositories;
using Kentico.AspNetCore.LocalizedRouting.Extensions;
using Kentico.Kontent.AspNetCore.ImageTransformation;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Extensions;
using Kentico.Kontent.Management.Helpers;
using Kentico.Kontent.Management.Helpers.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using static DancingGoat.Configuration.Constants;

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

            // Register the ImageTransformationOptions required by Kentico Kontent tag helpers
            services.Configure<ImageTransformationOptions>(Configuration.GetSection(nameof(ImageTransformationOptions)));

            // Configure the Edit Link Builder
            services.Configure<ManagementHelpersOptions>(Configuration.GetSection(nameof(DeliveryOptions)));
            services.AddSingleton<IEditLinkBuilder, EditLinkBuilder>();

            // Enable Delivery Client
            services.AddHttpClient<IDeliveryHttpClient, DeliveryHttpClient>();
            services.AddSingleton<ITypeProvider, CustomTypeProvider>();
            services.AddSingleton<IContentLinkUrlResolver, CustomContentLinkUrlResolver>();
            services.AddDeliveryClient(Configuration);

            // Register a second client for the configuration wizard
            services.AddDeliveryClient(ReferenceClient, Configuration, $"{nameof(AppConfiguration)}:{nameof(DeliveryOptions)}");

            // Repositories
            services.AddSingleton<ICafesRepository, CafesRepository>();

            // Configuration
            services.ConfigureWritable<AppConfiguration>((IConfigurationRoot)Configuration, Configuration.GetSection(nameof(AppConfiguration)));
            services.ConfigureWritable<DeliveryOptions>((IConfigurationRoot)Configuration, Configuration.GetSection(nameof(DeliveryOptions)));

            // I18N
            services.ConfigureRequestLocalization(DefaultCulture, SpanishCulture);
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
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
                app.UseExceptionHandler($"/{DefaultCulture}/Errors/NotFound");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute($"/{DefaultCulture}/Errors/NotFound");

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDynamicControllerRoute<CustomLocalizedRoutingTranslationTransformer>("{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
