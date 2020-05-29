using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.AspNetCore.LocalizedRouting;

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
            var translatedController = await _localizedRoutingProvider.ProvideRouteAsync(language, originalController, originalController, ProvideRouteType.OriginalToTranslated);
            var tranclatedAction = await _localizedRoutingProvider.ProvideRouteAsync(language, originalAction, originalController, ProvideRouteType.OriginalToTranslated);

            // Specific item is not selected, url will not be changed after redirect
            if (itemId == Guid.Empty || string.IsNullOrEmpty(itemType))
            {
                return RedirectToAction(tranclatedAction, translatedController, new { culture = language });
            }

            var item = (await _client.GetItemsAsync<object>(
                new SystemTypeEqualsFilter(itemType),
                new EqualsFilter("system.id", itemId.ToString()),
                new LanguageParameter(language),
                new ElementsParameter("url_pattern"))).Items.FirstOrDefault();

            if (!(item is IDetailItem detaiItem))
            {
                return NotFound();
            }


            return RedirectToAction(tranclatedAction, translatedController, new { culture = language, urlSlug = detaiItem.UrlPattern });
        }
    }
}