using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace DancingGoat.Helpers.Extensions
{
    public static class LocalizedRouteExtensions
    {

        // TODO : Implement MapLocalizedRoute()
        /*public static Route MapLocalizedRoute(this RouteCollection routes, string name, string url, object defaults, string language = null)
        {
            Route route = new Route(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(new { language = new LanguageConstraint { Language = language } }),
                new LocalizedMvcRouteHandler(LanguageClient.DEFAULT_LANGUAGE));

            routes.Add(name, route);
            return route;
        }*/
    }
}
