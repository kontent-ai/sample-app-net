using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("product-catalog/brewers")]
    public class BrewersController : AsyncController
    {
        private readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

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