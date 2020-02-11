using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Cafes")]
    [LocalizedRoute("es-ES", "Cafés")]
    public class CafesController : ControllerBase
    {
        public CafesController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }

        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Cafe>(
                new EqualsFilter("system.type", "cafe"),
                new OrderParameter("system.name"),
                new LanguageParameter(Language)
            );
            var cafes = response.Items;

            var viewModel = new CafesViewModel
            {
                CompanyCafes = cafes.Where(c => c.Country == "USA").ToList(),
                PartnerCafes = cafes.Where(c => c.Country != "USA").ToList()
            };

            return View(viewModel);
        }
    }
}