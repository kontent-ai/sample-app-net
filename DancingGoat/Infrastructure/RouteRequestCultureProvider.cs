using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace DancingGoat.Infrastructure;

public class RouteRequestCultureProvider: RequestCultureProvider
{

    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.ToString();
        var parts = path.Split('/');
        var culture = parts[1];
        return Task.FromResult(new ProviderCultureResult(culture));
    }
}