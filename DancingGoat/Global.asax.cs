using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DancingGoat
{
    public class DancingGoatApplication : System.Web.HttpApplication
    {
        public string AzureWebsiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
        //public bool

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void LoadSettings()
        {

        }
    }
}