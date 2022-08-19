﻿using DancingGoat.Configuration;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using DancingGoat.Repositories;
using AspNetCore.Mvc.Routing.Localization.Extensions;
using Kontent.Ai.AspNetCore.ImageTransformation;
using Kontent.Ai.Delivery;
using Kontent.Ai.Delivery.Abstractions;
using Kontent.Ai.Delivery.Extensions;
using Kontent.Ai.Management.Helpers;
using Kontent.Ai.Management.Helpers.Configuration;
using Kontent.Ai.Delivery.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using static DancingGoat.Configuration.Constants;
using System.Globalization;

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

            // Register the ImageTransformationOptions required by Kontent tag helpers
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
            services.AddDeliveryClient(ReferenceClient, Configuration, $"{nameof(AppConfiguration)}:{nameof(DeliveryOptions)}", NamedServiceProviderType.Autofac);

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
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("es-ES"),
            };
            services.AddLocalizedRouting(supportedCultures);
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
