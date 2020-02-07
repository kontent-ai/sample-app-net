using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Coffees")]
    [LocalizedRoute("es-ES", "Coffees")]
    public class CoffeesController : ControllerBase
    {
        public CoffeesController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }

        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
        public async Task<ActionResult> Index()
        {
            var itemsTask = _client.GetItemsAsync<Coffee>(
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter(Coffee.ImageCodename, Coffee.PriceCodename, Coffee.ProductStatusCodename, Coffee.ProductNameCodename, Coffee.UrlPatternCodename),
                new DepthParameter(0),
                new LanguageParameter(Language)
            );

            var processingTask = _client.GetTaxonomyAsync(Coffee.ProcessingCodename);
            var statusTask = _client.GetTaxonomyAsync(Coffee.ProductStatusCodename);

            var model = new CoffeesViewModel
            {
                Items = (await itemsTask).Items,
                Filter = new CoffeesFilterViewModel
                {
                    AvailableProcessings = GetTaxonomiesAsSelectList(await processingTask),
                    AvailableProductStatuses = GetTaxonomiesAsSelectList(await statusTask)
                }
            };

            return View(model);
        }

        [LocalizedRoute("en-US", "Filter")]
        [LocalizedRoute("es-ES", "Filter")]
        public async Task<ActionResult> Filter(CoffeesFilterViewModel model)
        {
            var parameters = new List<IQueryParameter> {
                new EqualsFilter("system.type", "coffee"),
                new OrderParameter("elements.product_name"),
                new ElementsParameter(Coffee.ImageCodename, Coffee.PriceCodename, Coffee.ProductStatusCodename, Coffee.ProductNameCodename, Coffee.UrlPatternCodename),
                new DepthParameter(0),
                new LanguageParameter(Language)
            };

            var filterProcessing = model.GetFilteredProcessings().ToArray();
            if (filterProcessing.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Coffee.ProcessingCodename}", filterProcessing));
            }

            var filterStatus = model.GetFilteredProductStatuses().ToArray();
            if (filterStatus.Any())
            {
                parameters.Add(new AnyFilter($"elements.{Coffee.ProductStatusCodename}", filterStatus));
            }

            var response = await _client.GetItemsAsync<Coffee>(parameters);

            return PartialView("ProductListing", response.Items);
        }

        private IList<SelectListItem> GetTaxonomiesAsSelectList(TaxonomyGroup taxonomyGroup)
        {
            return taxonomyGroup.Terms.Select(x => new SelectListItem { Text = x.Name, Value = x.Codename }).ToList();
        }
    }
}