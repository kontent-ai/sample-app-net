using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace DancingGoat.Localization
{
    public class LanguageConstraint : IRouteConstraint
    {
        private readonly HashSet<string> _allowedLanguages = new HashSet<string>(new[] {"en-us", "es-es"},
            StringComparer.InvariantCultureIgnoreCase);
        public string Language { get; set; }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            var language = values[parameterName].ToString().ToLower();
            var isLangugageMatch = string.IsNullOrEmpty(Language) || language == Language;

            return _allowedLanguages.Contains(language) && isLangugageMatch;
        }
    }
}