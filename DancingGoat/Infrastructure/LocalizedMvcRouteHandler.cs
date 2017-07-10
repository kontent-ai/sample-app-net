using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DancingGoat.Infrastructure
{
    public class LocalizedMvcRouteHandler : MvcRouteHandler
    {
        private readonly string _defaultLanguage;

        public LocalizedMvcRouteHandler(string defaultLanguage)    
        {
            _defaultLanguage = defaultLanguage; //TODO: decide if this is even necessary
        }
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // Get the requested culture from the route
            var cultureName = requestContext.RouteData.Values["language"]?.ToString() ?? "en-US";

            CultureInfo culture;
            try
            {
                //TODO: refactor this slightly
                culture = new CultureInfo(cultureName);
            }
            catch
            {
                culture = new CultureInfo("en-US");
            }

            // Set the culture
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            return base.GetHttpHandler(requestContext);
        }
    }
}