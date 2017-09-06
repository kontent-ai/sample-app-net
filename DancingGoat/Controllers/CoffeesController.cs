using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class CoffeesController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var itemsTask = client.GetItemsAsync<Coffee>(
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "url_pattern"),
                new DepthParameter(0)
            );

            var processingTask = client.GetTaxonomyAsync("processing_type");
            var statusTask = client.GetTaxonomyAsync("product_status");

            var model = new CoffeesViewModel
            {
                Items = (await itemsTask).Items,
                Filter = new CoffeesFilterViewModel
                {
                    AvailableProcessingTypes = GetTaxonomiesAsSelectList(await processingTask),
                    AvailableStatusTypes = GetTaxonomiesAsSelectList(await statusTask)
                }
            };

            return View(model);
        }

        public async Task<ActionResult> Filter(CoffeesFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter("image", "price", "product_status", "url_pattern"),
                new DepthParameter(0),
            };

            var filterProcessing = model.GetFilteredProcessingTypes().ToArray();
            if (filterProcessing.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Coffee.ProcessingTypeCodename}", filterProcessing));
            }

            var filterStatus = model.GetFilteredStatusTypes().ToArray();
            if (filterStatus.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Coffee.ProductStatusCodename}", filterStatus));
            }

            var response = await client.GetItemsAsync<Coffee>(parameters);

            return PartialView("ProductListing", response.Items);
        }

        private IList<SelectListItem> GetTaxonomiesAsSelectList(TaxonomyGroup taxonomyGroup)
        {
            return taxonomyGroup.Terms.Select(x => new SelectListItem { Text = x.Name, Value = x.Codename }).ToList();
        }
    }
}