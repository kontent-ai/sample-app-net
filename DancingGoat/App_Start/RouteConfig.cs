using System;
using System.Web.Mvc;
using System.Web.Routing;
using DancingGoat.Helpers.Extensions;
using DancingGoat.Localization;

namespace DancingGoat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapLocalizedRoute(
                name: "LanguageSelector",
                url: "{language}/languageselector/{originalController}/{originalAction}/{id}/{type}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, originalController = "Home", originalAction = "Index", controller = "LanguageSelector", action = "Index", id = Guid.Empty, type = "" }
            );
            routes.MapLocalizedRoute(
                name: "CoffeesCatalogSp",
                url: "{language}/tienda/cafe/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Coffees", action = "Index", urlSlug = UrlParameter.Optional },
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "CoffeesCatalog",
                url: "{language}/product-catalog/coffees/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Coffees", action = "Index", urlSlug = UrlParameter.Optional }
            );
            routes.MapLocalizedRoute(
                name: "BrewersCatalogSp",
                url: "{language}/tienda/cerveceros/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Brewers", action = "Index", urlSlug = UrlParameter.Optional },
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "BrewersCatalog",
                url: "{language}/product-catalog/brewers/{action}/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Brewers", action = "Index", urlSlug = UrlParameter.Optional }
            );
            routes.MapLocalizedRoute(
                name: "ArticlesSp",
                url: "{language}/articulos",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Articles", action = "Index" },
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "Articles",
                url: "{language}/articles",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Articles", action = "Index" }
            );
            routes.MapLocalizedRoute(
                name: "ProductSp",
                url: "{language}/producto/detalle/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Product", action = "Detail", urlSlug = "" },
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "Product",
                url: "{language}/product/detail/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Product", action = "Detail", urlSlug = "" }
            );
            routes.MapLocalizedRoute(
                name: "ArticleSp",
                url: "{language}/articulos/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Articles", action = "Show", urlSlug = "" },
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "Article",
                url: "{language}/articles/{urlSlug}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Articles", action = "Show", urlSlug = "" }
            );
            routes.MapLocalizedRoute(
                name: "AboutSp",
                url: "{language}/quienessomos",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "About", action = "Index"},
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "PartnershipSp",
                url: "{language}/afiliados",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Partnership", action = "Index"},
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "ContactsSp",
                url: "{language}/contactos",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Contacts", action = "Index" },
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "HomeSp",
                url: "{language}/inicio",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Home", action = "Index"},
                language: LanguageClient.SpanishLanguge
            );
            routes.MapLocalizedRoute(
                name: "Default",
                url: "{language}/{controller}/{action}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Home", action = "Index" }
            );

            // Display a custom view when no route is found
            routes.MapRoute(
                name: "Error",
                url: "{*url}",
                defaults: new { language = LanguageClient.DEFAULT_LANGUAGE, controller = "Errors", action = "NotFound" }
            );

        }
    }
}