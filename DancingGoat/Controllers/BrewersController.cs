using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("product-catalog/brewers")]
    public class BrewersController : ControllerBase
    {
        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync<Brewer>(
                new EqualsFilter("system.type", "brewer"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "processing"),
                new DepthParameter(0)
            );

            return View(response.Items);
        }

        public async Task<ActionResult> Filter(BrewerFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "brewer"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "processing"),
                new DepthParameter(0)
            };

            var manufacturers = model.GetManufacturerFilters().ToArray();
            if (manufacturers.Any())
            {
                parameters.Add(new InFilter("elements.manufacturer", manufacturers));
            }

            var response = await client.GetItemsAsync<Brewer>(parameters);

            return PartialView("ProductListing", response.Items.Cast<Product>());
        }
    }
}