using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("contacts")]
    public class ContactsController : ControllerBase
    {
        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync<Cafe>(
                new EqualsFilter("system.type", "cafe"),
                new EqualsFilter("elements.country", "USA")
            );
            var cafes = response.Items;

            var viewModel = new ContactsViewModel
            {
                Roastery = cafes.FirstOrDefault(),
                Cafes = cafes
            };

            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult CompanyAddress()
        {
            var contact = Task.Run(() => client.GetItemAsync<Home>("home", new ElementsParameter("contact"))).Result.Item.Contact;

            return Content(contact);
        }
    }
}