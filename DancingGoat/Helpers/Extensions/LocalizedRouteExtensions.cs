using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DancingGoat.Localization;

namespace DancingGoat.Helpers.Extensions
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class LocalizedRouteExtensions
    {
        public static Route MapLocalizedRoute(this RouteCollection routes, string name, string url, object defaults, string language = null)
        {
            Route route = new Route(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(new { language = new LanguageConstraint { Language = language } }),
                new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE));

            routes.Add(name, route);
            return route;
        }
     }
}