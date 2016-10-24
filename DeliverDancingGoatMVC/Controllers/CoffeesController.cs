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
    [RoutePrefix("product-catalog/coffees")]
    public class CoffeesController : AsyncController
    {
        private readonly DeliverClient client = new DeliverClient(ConfigurationManager.AppSettings["ProjectId"], ConfigurationManager.AppSettings["PreviewToken"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "coffee"),
                new Order("elements.product_name"),
                new ElementsFilter("image", "price", "product_status", "processing"),
                new DepthFilter(0)
            };

            var response = await client.GetItemsAsync(filters);

            return View(response.Items);
        }

        public async Task<ActionResult> Filter (CoffeesFilterViewModel model)
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "coffee"),
                new Order("elements.product_name"),
                new ElementsFilter("image", "price", "product_status", "processing"),
                new DepthFilter(0),
            };

            var filter = model.GetFilteredValues().ToArray();
            if (filter.Any())
            {
                filters.Add(new InFilter("elements.processing", filter));
            }

            var response = await client.GetItemsAsync(filters);

            return PartialView("CoffeeList", response.Items);
        }
    }
}