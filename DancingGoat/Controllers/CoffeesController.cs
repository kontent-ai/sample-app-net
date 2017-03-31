using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("product-catalog/coffees")]
    public class CoffeesController : ControllerBase
    {
        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync<Coffee>(
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "processing"),
                new DepthParameter(0)
            );

            return View(response.Items);
        }

        public async Task<ActionResult> Filter(CoffeesFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "processing"),
                new DepthParameter(0),
            };

            var filter = model.GetFilteredValues().ToArray();
            if (filter.Any())
            {
                parameters.Add(new InFilter("elements.processing", filter));
            }

            var response = await client.GetItemsAsync<Coffee>(parameters);

            return PartialView("ProductListing", response.Items.Cast<Product>());
        }
    }
}