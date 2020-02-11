using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{

    [LocalizedRoute("en-US", "Contacts")]
    [LocalizedRoute("es-ES", "Contacto")]
    public class ContactsController : ControllerBase
    {
        public ContactsController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }

        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Cafe>(
                new EqualsFilter("system.type", "cafe"),
                new EqualsFilter("elements.country", "USA"),
                new LanguageParameter(Language)
            );
            var cafes = response.Items;

            var viewModel = new ContactsViewModel
            {
                Roastery = cafes.FirstOrDefault(),
                Cafes = cafes
            };

            return View(viewModel);
        }
    }
}