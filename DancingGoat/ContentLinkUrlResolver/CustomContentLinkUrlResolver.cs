using DancingGoat.Models;
using Kontent.Ai.Delivery.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace DancingGoat
{
    public class CustomContentLinkUrlResolver : IContentLinkUrlResolver
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;

        public CustomContentLinkUrlResolver(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<string> ResolveLinkUrlAsync(IContentLink link)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            switch (link.ContentTypeCodename)
            {
                case AboutUs.Codename:
                case FactAboutUs.Codename:
                    return await ResolveLink("Index", "About");
                case Article.Codename:
                    return await ResolveLink("Show", "Articles", new { urlSlug = link.UrlSlug });
                case Brewer.Codename:
                case Coffee.Codename:
                    return await ResolveLink("Detail", "Product", new { urlSlug = link.UrlSlug });
                case Cafe.Codename:
                    return await ResolveLink("Index", "Cafes");
                case Home.Codename:
                    return await ResolveLink("Index", "Home");
                default:
                    return urlHelper.Action("NotFound", "Errors");
            }
        }

        public Task<string> ResolveBrokenLinkUrlAsync()
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            return Task.FromResult(urlHelper.Action("NotFound", "Errors"));
        }

        private async Task<string> ResolveLink(string action, string controller, object data = null)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);

            return data == null ? urlHelper.Action(action, controller) : urlHelper.Action(action, controller, data);
        }

        private IUrlHelper GetHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
    }
}