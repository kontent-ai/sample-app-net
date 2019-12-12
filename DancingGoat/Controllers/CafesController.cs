using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Controllers
{
    public class CafesController : ControllerBase
    {
        public CafesController(IOptionsSnapshot<DeliveryOptions> deliveryOptions, IAppSettingProvider settingProvider, IDeliveryClient client) : base(deliveryOptions, settingProvider, client)
        {
        }
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Cafe>(
                new EqualsFilter("system.type", "cafe"),
                new OrderParameter("system.name")
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