using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kontent.Ai.Delivery.Abstractions;
using AspNetCore.Mvc.Routing.Localization;
using Kontent.Ai.Urls.Delivery.QueryParameters;
using Kontent.Ai.Urls.Delivery.QueryParameters.Filters;

namespace DancingGoat.Controllers
{
    public class LanguagesController : ControllerBase
    {
        private readonly ILocalizedRoutingProvider _localizedRoutingProvider;

        public LanguagesController(IDeliveryClientFactory deliveryClientFactory,
            ILocalizedRoutingProvider localizedRoutingProvider) : base(deliveryClientFactory)
        {
            _localizedRoutingProvider = localizedRoutingProvider;
        }

        public async Task<ActionResult> Index([FromQuery]Guid itemId, [FromQuery]string originalAction, [FromQuery]string itemType, [FromQuery]string originalController, [FromQuery]string language)
        {
            var translatedRouteInfo = await _localizedRoutingProvider.ProvideRouteAsync(language, originalController, originalAction, LocalizationDirection.OriginalToTranslated);

            if(translatedRouteInfo == null)
            {
                return NotFound();
            }

            // Specific item is not selected, url will not be changed after redirect
            if (itemId == Guid.Empty || string.IsNullOrEmpty(itemType))
            {
                return RedirectToAction(translatedRouteInfo.Action, translatedRouteInfo.Controller, new { culture = language });
            }

            var item = (await _client.GetItemsAsync<object>(
                new SystemTypeEqualsFilter(itemType),
                new EqualsFilter("system.id", itemId.ToString()),
                new LanguageParameter(language),
                new ElementsParameter("url_pattern"))).Items.FirstOrDefault();

            if (item is not IDetailItem detaiItem)
            {
                return NotFound();
            }


            return RedirectToAction(translatedRouteInfo.Action, translatedRouteInfo.Controller, new { culture = language, urlSlug = detaiItem.UrlPattern });
        }
    }
}