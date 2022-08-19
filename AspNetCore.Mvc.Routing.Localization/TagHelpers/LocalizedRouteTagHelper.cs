using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Routing.Localization.TagHelpers
{
    public class LocalizedRouteTagHelper : AnchorTagHelper
    {
        private readonly IActionContextAccessor _contextAccessor;
        private readonly ILocalizedRoutingProvider _localizedRoutingProvider;

        public LocalizedRouteTagHelper(IHtmlGenerator generator, IActionContextAccessor contextAccessor, ILocalizedRoutingProvider localizedRoutingProvider) : base(generator)
        {
            _contextAccessor = contextAccessor;
            _localizedRoutingProvider = localizedRoutingProvider;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            var culture = _contextAccessor.ActionContext.RouteData.Values["culture"]?.ToString();
            var routeInformationMetadata = await _localizedRoutingProvider.ProvideRouteAsync(culture, Controller, Action, LocalizationDirection.OriginalToTranslated);
            if (routeInformationMetadata != null)
            {
                Controller = routeInformationMetadata.Controller;
                Action = routeInformationMetadata.Action;
            }

            await base.ProcessAsync(context, output);
        }
    }
}
