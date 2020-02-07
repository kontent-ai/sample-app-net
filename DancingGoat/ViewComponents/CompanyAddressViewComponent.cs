using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DancingGoat.ViewComponents
{
    public class CompanyAddressViewComponent : ViewComponent
    {
        public IDeliveryClient DeliveryClient { get; }

        public CompanyAddressViewComponent(IDeliveryClientFactory deliveryClientFactory)
        {
            DeliveryClient = deliveryClientFactory.Get();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contact = await DeliveryClient.GetItemAsync<Home>("home", new ElementsParameter("contact"));
            return View(contact.Item.Contact);
        }
    }
}
