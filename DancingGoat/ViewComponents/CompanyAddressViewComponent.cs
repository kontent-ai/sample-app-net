using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat.ViewComponents
{
    public class CompanyAddressViewComponent : ViewComponent
    {
        public IDeliveryClient DeliveryClient { get; }

        public CompanyAddressViewComponent(IDeliveryClient deliveryClient)
        {
            DeliveryClient = deliveryClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contact = await DeliveryClient.GetItemAsync<Home>("home", new ElementsParameter("contact"));            
            return View(contact.Item.Contact);
        }
    }
}
