using DancingGoat.Models;
using Kontent.Ai.Delivery.Abstractions;
using Kontent.Ai.Urls.Delivery.QueryParameters;
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
            var contact = await DeliveryClient.GetItemAsync<Home>("home", new ElementsParameter(Home.ContactCodename));
            return View(contact.Item.Contact);
        }
    }
}
