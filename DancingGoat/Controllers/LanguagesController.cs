using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.AspNetCore.LocalizedRouting;
using Kentico.AspNetCore.LocalizedRouting.Attributes;

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

        public async Task<ActionResult> Index(Guid id, string originalAction, string type, string originalController, string language)
        {
            // Specific item is not selected, url will not be changed after redirect
            if (id == Guid.Empty || string.IsNullOrEmpty(type))
            {
                return RedirectToAction(originalAction, originalController, new { culture = language });
            }

            var item = (await _client.GetItemsAsync<object>(
                new SystemTypeEqualsFilter(type),
                new EqualsFilter("system.id", id.ToString()),
                new LanguageParameter(language),
                new ElementsParameter("url_pattern"))).Items.FirstOrDefault();

            if (!(item is IDetailItem detaiItem))
            {
                return NotFound();
            }

            var translatedController = await _localizedRoutingProvider.ProvideRouteAsync(language, originalController, ProvideRouteType.OriginalToTranslated);
            var tranclatedAction = await _localizedRoutingProvider.ProvideRouteAsync(language, originalAction, ProvideRouteType.OriginalToTranslated);

            return RedirectToAction(tranclatedAction, translatedController, new { culture = language, urlSlug = detaiItem.UrlPattern });
        }
    }
}