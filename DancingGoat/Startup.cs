using System.IO;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc(routes=>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action/Index}/{id?}");

                // Replacing Areas/Admin/AdminAreaRegistration.cs
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
