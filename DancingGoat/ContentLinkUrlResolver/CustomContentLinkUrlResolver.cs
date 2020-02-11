using DancingGoat.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Globalization;

namespace DancingGoat
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        protected string CurrentCulture => CultureInfo.CurrentUICulture.Name;

        public CustomContentLinkUrlResolver(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }
       
        public string ResolveLinkUrl(ContentLink link)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);

            switch (link.ContentTypeCodename)
            {
                case AboutUs.Codename:
                case FactAboutUs.Codename:
                    return urlHelper.Action("Index", "About");
                case Article.Codename:
                    return urlHelper.Action("Show", "Articles", new { urlSlug = link.UrlSlug });
                case Brewer.Codename:
                case Coffee.Codename:
                    return urlHelper.Action("Detail", "Product", new { urlSlug = link.UrlSlug });
                case Cafe.Codename:
                    return urlHelper.Action("Index", "Cafes");
                case Home.Codename:
                    return urlHelper.Action("Index", "Home");
                default:
                    return urlHelper.Action("NotFound", "Errors");
            }
        }

        public string ResolveBrokenLinkUrl()
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            return urlHelper.Action("NotFound", "Errors");
        }

        private IUrlHelper GetHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
    }
}