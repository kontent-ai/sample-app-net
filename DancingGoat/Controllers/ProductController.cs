using Kentico.Kontent.Delivery;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{

    [LocalizedRoute("en-US", "Product")]
    [LocalizedRoute("es-ES", "Tienda")]
    public class ProductController : ControllerBase
    {
        public ProductController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }

        [LocalizedRoute("en-US", "Detail")]
        [LocalizedRoute("es-ES", "Detail")]
        public async Task<ActionResult> Detail(string urlSlug)
        {
            var item = (await _client.GetItemsAsync<object>(new EqualsFilter("elements.url_pattern", urlSlug), new InFilter("system.type", "brewer", "coffee"), new LanguageParameter(Language))).Items.FirstOrDefault();

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