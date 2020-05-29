using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat.Infrastructure
{
    public class CultureValidatorActionFilter : ActionFilterAttribute
    {
        private readonly string[] _cultures = { "en-US", "es-ES" };
        public CultureValidatorActionFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentCulture = context.RouteData.Values["culture"];
            if (!_cultures.Contains(currentCulture))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Errors", action = "NotFound", culture = "en-US" })
                );
            }
            base.OnActionExecuting(context);
        }
    }
}
