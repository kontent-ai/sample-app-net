﻿using DancingGoat.Models;
using AspNetCore.Mvc.Routing.Localization;
using Kontent.Ai.Delivery.Abstractions;
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

        public async Task<string> ResolveLinkUrlAsync(IContentLink link)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            switch (link.ContentTypeCodename)
            {
                case AboutUs.Codename:
                case FactAboutUs.Codename:
                    return await TranslateLink("Index", "About");
                case Article.Codename:
                    return await TranslateLink("Show", "Articles", new { urlSlug = link.UrlSlug });
                case Brewer.Codename:
                case Coffee.Codename:
                    return await TranslateLink("Detail", "Product", new { urlSlug = link.UrlSlug });
                case Cafe.Codename:
                    return await TranslateLink("Index", "Cafes");
                case Home.Codename:
                    return await TranslateLink("Index", "Home");
                default:
                    return urlHelper.Action("NotFound", "Errors");
            }
        }

        public Task<string> ResolveBrokenLinkUrlAsync()
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            return Task.FromResult(urlHelper.Action("NotFound", "Errors"));
        }

        private async Task<string> TranslateLink(string action, string controller, object data = null)
        {
            var urlHelper = GetHelper(_urlHelperFactory, _actionContextAccessor);
            var culture = _actionContextAccessor.ActionContext.RouteData.Values["culture"].ToString();

            var translatedRouteInfo = await _localizedRoutingProvider.ProvideRouteAsync(culture, controller, action, LocalizationDirection.OriginalToTranslated);

            if (data == null)
            {
                return urlHelper.Action(translatedRouteInfo.Action, translatedRouteInfo.Controller);
            }

            return urlHelper.Action(translatedRouteInfo.Action, translatedRouteInfo.Controller, data);
        }

        private IUrlHelper GetHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor)
        {
            return urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }
    }
}