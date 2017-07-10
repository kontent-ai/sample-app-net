using System.Web.Mvc;
using System.Web.Routing;
using DancingGoat.Infrastructure;

namespace DancingGoat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var route = routes.MapRoute(
                name: "CoffeesCatalog",  
                url: "{language}/product-catalog/coffees/{action}/{urlSlug}",
                defaults: new { language = "en-us", controller = "Coffees", action = "Index", urlSlug = UrlParameter.Optional },
                constraints: new { language = @"\w\w-\w\w" }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler("en-US");

            route = routes.MapRoute(
                name: "BrewersCatalog",
                url: "{language}/product-catalog/brewers/{action}/{urlSlug}",
                defaults: new { language = "en-us", controller = "Brewers", action = "Index", urlSlug = UrlParameter.Optional },
                constraints: new { language = @"\w\w-\w\w" }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler("en-US");

            route = routes.MapRoute(
                name: "Articles",
                url: "{language}/articles",
                defaults: new { language = "en-us", controller = "Articles", action = "Index" },
                constraints: new { language = @"\w\w-\w\w" }
            );
            route.RouteHandler = new LocalizedMvcRouteHandler("en-US");
            route = routes.MapRoute(
                name: "Article",
                url: "{language}/articles/{urlSlug}",
                defaults: new { language = "en-us", controller = "Articles", action = "Show", urlSlug = ""},
                constraints: new { language = @"\w\w-\w\w" }
);
            route.RouteHandler = new LocalizedMvcRouteHandler("en-US");

            route = routes.MapRoute(
                name: "LocalizedContent", 
                url: "{language}/{controller}/{action}/{urlSlug}",
                defaults: new { language = "en-us", controller = "Home", action = "Index", urlSlug = UrlParameter.Optional},
                constraints: new { language = @"\w\w-\w\w"}
            );
            route.RouteHandler = new LocalizedMvcRouteHandler("en-US");

            // Display a custom view when no route is found
            routes.MapRoute(
                name: "Error",
                url: "{*url}",
                defaults: new { language = "en-US", controller = "Errors", action = "NotFound" }
            );

        }
    }
}