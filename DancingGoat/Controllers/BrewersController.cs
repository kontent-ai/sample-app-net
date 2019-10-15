using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class BrewersController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var itemsTask = client.GetItemsAsync<Brewer>(
                new EqualsFilter("system.type", "brewer"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter(Brewer.ImageCodename, Brewer.PriceCodename, Brewer.ProductStatusCodename, Brewer.ProductNameCodename, Brewer.UrlPatternCodename),
                new DepthParameter(0)
            );

            var statusTask = client.GetTaxonomyAsync(Brewer.ProductStatusCodename);
            var manufacturerTask = client.GetTaxonomyAsync(Brewer.ManufacturerCodename);

            var model = new BrewersViewModel
            {
                Items = (await itemsTask).Items,
                Filter = new BrewerFilterViewModel
                {
                    AvailableProductStatuses = GetTaxonomiesAsSelectList(await statusTask),
                    AvailableManufacturers = GetTaxonomiesAsSelectList(await manufacturerTask)
                }
            };

            return View(model);
        }

        public async Task<ActionResult> Filter(BrewerFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "brewer"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter(Brewer.ImageCodename, Brewer.PriceCodename, Brewer.ProductStatusCodename, Brewer.ProductNameCodename, Brewer.UrlPatternCodename),
                new DepthParameter(0)
            };

            var manufacturers = model.GetFilteredManufacturers().ToArray();
            if (manufacturers.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Brewer.ManufacturerCodename}", manufacturers));
            }

            var statusTypes = model.GetFilteredProductStatuses().ToArray();
            if (statusTypes.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Brewer.ProductStatusCodename}", statusTypes));
            }

            var response = await client.GetItemsAsync<Brewer>(parameters);

            return PartialView("ProductListing", response.Items);
        }

        private IList<SelectListItem> GetTaxonomiesAsSelectList(TaxonomyGroup taxonomyGroup)
        {
            return taxonomyGroup.Terms.Select(x => new SelectListItem{Text = x.Name, Value = x.Codename}).ToList();
        }
    }
}