using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DancingGoat.Models;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Controllers
{
    public class HomeController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemAsync<Home>("home");

            var viewModel = new HomeViewModel
            {
                ContentItem = response.Item,
                Header = response.Item.HeroUnit.Cast<HeroUnit>().FirstOrDefault(x => x.System.Codename == "home_page_hero_unit"),
            };

            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult CompanyAddress()
        {
            var contact = Task.Run(() => client.GetItemAsync<Home>("home", new ElementsParameter("contact"))).Result.Item.Contact;

            return PartialView("CompanyAddress", contact);
        }
    }
}
