using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kontent.Ai.Delivery.Abstractions;
using DancingGoat.Models;
using DancingGoat.Configuration;
using Kontent.Ai.Urls.Delivery.QueryParameters.Filters;
using Kontent.Ai.Urls.Delivery.QueryParameters;

namespace DancingGoat.Controllers
{

    public class ProductController : ControllerBase
    {
        public ProductController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }
        
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