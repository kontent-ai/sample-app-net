using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace DancingGoat.Infrastructure
{
    public class CultureValidatorActionFilter : ActionFilterAttribute
    {
        public IOptions<RequestLocalizationOptions> Options { get; }

        public CultureValidatorActionFilter(IOptions<RequestLocalizationOptions> options)
        {
            Options = options;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentCulture = context.RouteData.Values["culture"] as string;
            if (!Options.Value.SupportedCultures.Contains(new System.Globalization.CultureInfo(currentCulture)))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Errors", action = "NotFound", culture = Options.Value.DefaultRequestCulture.Culture.Name })
                );
            }
            base.OnActionExecuting(context);
        }
    }
}
