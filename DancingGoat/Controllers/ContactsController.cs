using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DancingGoat.Controllers
{
    public class ContactsController : ControllerBase
    {
        public ContactsController(IOptionsSnapshot<DeliveryOptions> deliveryOptions, IAppSettingProvider settingProvider, IDeliveryClient client) : base(deliveryOptions, settingProvider, client)
        {
        }
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Cafe>(
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
    }
}