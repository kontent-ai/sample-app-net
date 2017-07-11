using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DancingGoat.Localization
{
    public class LocalizedMvcRouteHandler : MvcRouteHandler
    {
        private readonly string _defaultLanguage;

        public LocalizedMvcRouteHandler(string defaultLanguage)    
        {
            _defaultLanguage = defaultLanguage;
        }
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            // Get the requested culture from the route
            var cultureName = requestContext.RouteData.Values["language"]?.ToString() ?? _defaultLanguage;

            CultureInfo culture;
            try
            {
                culture = new CultureInfo(cultureName);
            }
            catch
            {
                culture = new CultureInfo(_defaultLanguage);
            }

            // Set the culture
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            return base.GetHttpHandler(requestContext);
        }
    }
}