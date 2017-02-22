using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("contacts")]
    public class ContactsController : AsyncController
    {
        private readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync(
                new EqualsFilter("system.type", "cafe"),
                new EqualsFilter("elements.country", "USA")
            );
            var cafes = response.Items;

            var viewModel = new ContactsViewModel
            {
                Roastery = cafes.First(),
                Cafes = cafes
            };

            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult CompanyAddress()
        {
            var contact = Task.Run(() => client.GetItemAsync("home", new ElementsParameter("contact"))).Result.Item.GetString("contact");

            return Content(contact);
        }
    }
}