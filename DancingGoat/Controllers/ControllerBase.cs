using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using DancingGoat.Infrastructure;
using DancingGoat.Localization;

namespace DancingGoat.Controllers
{
    [TypeFilter(typeof(SelfConfigActionFilterAttribute))]
    public class ControllerBase : Controller
    {
        protected readonly IDeliveryClient _client;

        public ControllerBase(IDeliveryClientFactory deliveryClientFactory)
        {
            _client = deliveryClientFactory.GetDeliveryClient();
        }
    }
}

