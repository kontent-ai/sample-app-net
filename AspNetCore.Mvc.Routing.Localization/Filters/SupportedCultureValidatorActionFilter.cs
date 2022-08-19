using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;

namespace AspNetCore.Mvc.Routing.Localization.Filters
{
    /// <summary>
    /// This filter validates supported culture 
    /// </summary>
    public class SupportedCultureValidatorActionFilter : ActionFilterAttribute
    {
        private IOptions<RequestLocalizationOptions> _options { get; set; }

        public SupportedCultureValidatorActionFilter(IOptions<RequestLocalizationOptions> options)
        {
            _options = options;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var culture = context.RouteData.Values["culture"] as string;

            if (string.IsNullOrEmpty(culture) || !_options.Value.SupportedCultures.Contains(CultureInfo.GetCultureInfo(culture)))
            {
                throw new ArgumentException("The request does not contain a culture");
            }

            base.OnActionExecuting(context);
        }
    }
}
