using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;

using DeliverDancingGoatMVC.Models;
using KenticoCloud.Deliver;

namespace DeliverDancingGoatMVC.Controllers
{
    [RoutePrefix("product-catalog/brewers")]
    public class BrewersController : AsyncController
    {
        private readonly DeliverClient client = new DeliverClient(ConfigurationManager.AppSettings["ProjectId"], ConfigurationManager.AppSettings["PreviewToken"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "brewer"),
                new Order("elements.product_name"),
                new ElementsFilter("image", "price", "product_status", "processing"),
                new DepthFilter(0)
            };

            var response = await client.GetItemsAsync(filters);

            return View(response.Items);
        }

        public async Task<ActionResult> Filter(BrewerFilterViewModel model)
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "brewer"),
                new Order("elements.product_name"),
                new ElementsFilter("image", "price", "product_status", "processing"),
                new DepthFilter(0)
            };

            var manufacturers = model.GetManufacturerFilters().ToArray();
            if (manufacturers.Any())
            {
                filters.Add(new InFilter("elements.manufacturer", manufacturers));
            }

            var response = await client.GetItemsAsync(filters);

            return PartialView("BrewersList", response.Items);
        }
    }
}