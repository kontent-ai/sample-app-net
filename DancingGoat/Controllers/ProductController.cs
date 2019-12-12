using System;
using System.Collections.Generic;
using Kentico.Kontent.Delivery;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DancingGoat.Controllers
{
    public class ProductController : ControllerBase
    {
        public ProductController(IOptionsSnapshot<DeliveryOptions> deliveryOptions, IAppSettingProvider settingProvider, IDeliveryClient client) : base(deliveryOptions, settingProvider, client)
        {
        }

        public async Task<ActionResult> Detail(string urlSlug)
        {
            var item = (await _client.GetItemsAsync<object>(new EqualsFilter("elements.url_pattern", urlSlug), new InFilter("system.type", "brewer", "coffee"))).Items.FirstOrDefault();

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