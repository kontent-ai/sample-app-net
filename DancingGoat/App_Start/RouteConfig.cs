using System.Web.Mvc;
using System.Web.Routing;

namespace DancingGoat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Error",
                url: "Errors/{error}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
        }
    }
}