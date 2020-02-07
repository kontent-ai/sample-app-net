using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    public class LanguagesController : ControllerBase
    {
        public LanguagesController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {

        }

        public async Task<ActionResult> Index(Guid id, string originalAction, string type, string originalController)
        {
            // Specific item is not selected, url will not be changed after redirect
            if (id == Guid.Empty || string.IsNullOrEmpty(type))
            {
                return RedirectToAction(originalAction, originalController);
            }

            var item = (await _client.GetItemsAsync<object>(
                new EqualsFilter("system.type", type),
                new EqualsFilter("system.id", id.ToString()),
                new ElementsParameter("url_pattern"))).Items.FirstOrDefault();

            if (!(item is IDetailItem detaiItem))
            {
                return NotFound();
            }

            return RedirectToAction(originalAction, originalController, new { urlSlug = detaiItem.UrlPattern });
        }
    }
}