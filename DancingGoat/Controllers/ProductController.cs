using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Delivery.Abstractions;
using DancingGoat.Models;
using DancingGoat.Configuration;
using Kentico.Kontent.Delivery.Urls.QueryParameters.Filters;
using Kentico.Kontent.Delivery.Urls.QueryParameters;

namespace DancingGoat.Controllers
{

    [LocalizedRoute(Constants.EnglishCulture, "Product")]
    [LocalizedRoute(Constants.SpanishCulture, "Tienda")]
    public class ProductController : ControllerBase
    {
        public ProductController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }

        [LocalizedRoute(Constants.EnglishCulture, "Detail")]
        [LocalizedRoute(Constants.SpanishCulture, "Detalle")]
        public async Task<ActionResult> Detail(string urlSlug)
        {
            var item = (await _client.GetItemsAsync<object>(new EqualsFilter("elements.url_pattern", urlSlug), new InFilter("system.type", Brewer.Codename, Coffee.Codename), new LanguageParameter(Language))).Items.FirstOrDefault();

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.FreeTasteRequested = TempData["formSubmited"] ?? false;
                ViewBag.UrlSlug = urlSlug;
                return View(item.GetType().Name, item);
            }
        }

        /// <summary>
        /// Dummy action.
        /// </summary>
        [HttpPost]
        public ActionResult FreeTaste()
        {
            // If needed, put your code here to work with the uploaded data in MVC.
            TempData["formSubmited"] = true;
            return RedirectToAction("Detail", new { urlSlug = Request.Form["product_url_slug"] });
        }
    }
}