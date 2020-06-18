using DancingGoat.Configuration;
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
        public CultureValidatorActionFilter()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var currentCulture = context.RouteData.Values["culture"];
            if (!CultureConstants.Cultures.Contains(currentCulture))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Errors", action = "NotFound", culture = "en-US" })
                );
            }
            base.OnActionExecuting(context);
        }
    }
}
