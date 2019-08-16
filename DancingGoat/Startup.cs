using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Owin;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KenticoCloud.Delivery;

namespace DancingGoat
{
    public class Startup
    {

        public Startup(IHostingEnvironment enviroment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(enviroment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{enviroment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDeliveryClient(Configuration);
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddRouting();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                routes.MapAreaRoute(
                
                    name: "Admin",
                    areaName: "Admin_default",
                    template: "Admin/{controller}/{action}/{id}"

                );
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
