using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IOptionsSnapshot<DeliveryOptions> deliveryOptions, IAppSettingProvider settingProvider, IDeliveryClient client) 
            : base(deliveryOptions, settingProvider, client)
        {
        }

        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemAsync<Home>("home");

            var viewModel = new HomeViewModel
            {
                ContentItem = response.Item,
                Header = response.Item.HeroUnit.Cast<HeroUnit>().FirstOrDefault(x => x.System.Codename == "home_page_hero_unit"),
            };

            return View(viewModel);
        }
    }
}