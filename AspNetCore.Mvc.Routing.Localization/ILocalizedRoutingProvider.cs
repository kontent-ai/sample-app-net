using AspNetCore.Mvc.Routing.Localization.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Routing.Localization
{
    public interface ILocalizedRoutingProvider
    {
        Task<RouteInformation> ProvideRouteAsync(string culture, string controller, string action, LocalizationDirection direction);
        internal IEnumerable<LocalizedRoute> Routes { get; }
    }
}