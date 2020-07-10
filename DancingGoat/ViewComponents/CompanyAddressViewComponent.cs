using DancingGoat.Models;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Delivery.Urls.QueryParameters;
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
