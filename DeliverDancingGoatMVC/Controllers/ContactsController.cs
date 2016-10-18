using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using DeliverDancingGoatMVC.Models;
using KenticoCloud.Deliver;

namespace DeliverDancingGoatMVC.Controllers
{
    [RoutePrefix("contacts")]
    public class ContactsController : AsyncController
    {
        private readonly DeliverClient client = new DeliverClient(ConfigurationManager.AppSettings["ProjectId"], ConfigurationManager.AppSettings["PreviewToken"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "cafe"),
                new EqualsFilter("elements.country", "USA")
            };

            var cafes = (await client.GetItemsAsync(filters)).Items;

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
            var filters = new List<IFilter> {
                new ElementsFilter("contact")
            };

            var contact = Task.Run(() => client.GetItemAsync("home", filters)).Result.Item.GetString("contact");

            return Content(contact);
        }
    }
}