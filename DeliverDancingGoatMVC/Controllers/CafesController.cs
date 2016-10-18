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
    [RoutePrefix("cafes")]
    public class CafesController : AsyncController
    {
        private readonly DeliverClient client = new DeliverClient(ConfigurationManager.AppSettings["ProjectId"], ConfigurationManager.AppSettings["PreviewToken"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var filters = new List<IFilter> {
                new EqualsFilter("system.type", "cafe"),
                new Order("system.name")
            };

            var cafes = (await client.GetItemsAsync(filters)).Items;

            var viewModel = new CafesViewModel
            {
                CompanyCafes = cafes.Where(c => c.GetString("country") == "USA").ToList(),
                PartnerCafes = cafes.Where(c => c.GetString("country") != "USA").ToList()
            };

            return View(viewModel);
        }
    }
}