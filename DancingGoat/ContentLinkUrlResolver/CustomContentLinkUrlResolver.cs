using DancingGoat.Models;
using Kentico.AspNetCore.LocalizedRouting;
using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Globalization;
using System.Threading.Tasks;

namespace DancingGoat
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizedRoutingProvider _localizedRoutingProvider;

        protected string CurrentCulture => CultureInfo.CurrentUICulture.Name;

        public CustomContentLinkUrlResolver(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, ILocalizedRoutingProvider localizedRoutingProvider)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
            _localizedRoutingProvider = localizedRoutingProvider;
            
        }

        public string ResolveLinkUrl(ContentLink link)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            switch (link.ContentTypeCodename)
            {
                case AboutUs.Codename:
                case FactAboutUs.Codename:
                    return TranslateLink("Index", "About").Result;
                case Article.Codename:
                    return TranslateLink("Show", "Articles", new { urlSlug = link.UrlSlug }).Result;
                case Brewer.Codename:
                case Coffee.Codename:
                    return TranslateLink("Detail", "Product", new { urlSlug = link.UrlSlug }).Result;
                case Cafe.Codename:
                    return TranslateLink("Index", "Cafes").Result;
                case Home.Codename:
                    return TranslateLink("Index", "Home").Result;
                default:
                    return urlHelper.Action("NotFound", "Errors");
            }
        }

        public string ResolveBrokenLinkUrl()
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            return urlHelper.Action("NotFound", "Errors");
        }

        private async Task<string> TranslateLink(string action, string controller, object data = null)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            var culture = _actionContextAccessor.ActionContext.RouteData.Values["culture"].ToString();

            var translatedAction = await _localizedRoutingProvider.ProvideRouteAsync(culture, action, controller, ProvideRouteType.OriginalToTranslated);
            var translatedController = await _localizedRoutingProvider.ProvideRouteAsync(culture, controller, controller, ProvideRouteType.OriginalToTranslated);

            if (data == null)
            {
                return urlHelper.Action(translatedAction, translatedController);
            }

            return urlHelper.Action(translatedAction, translatedController, data);
        }

        private IUrlHelper GetHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
    }
}