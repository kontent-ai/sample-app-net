using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DancingGoat.ViewComponents
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        public IDeliveryClient DeliveryClient { get; }

        public LanguageSelectorViewComponent(IDeliveryClient deliveryClient)
        {
            DeliveryClient = deliveryClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var langs = await DeliveryClient.GetLanguagesAsync();            
            return View(langs.Languages);
        }
    }
}
