using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Routing.Localization
{
    public interface ILocalizedRoutingDynamicRouteValueResolver
    {
        Task<RouteValueDictionary> ResolveAsync(RouteValueDictionary values);
    }
}