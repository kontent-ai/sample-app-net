using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace DancingGoat.Infrastructure;

public class RouteRequestCultureProvider: RequestCultureProvider
{
    private readonly List<string> _supportedCultures = new() { "en-US", "es-ES" };
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.ToString();
        var culture = path.Split('/')[1];

        if (culture == "" || !_supportedCultures.Contains(culture))
        {
            culture = "en-US";
        }

        return Task.FromResult(new ProviderCultureResult(culture));
    }
}