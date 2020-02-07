using Microsoft.AspNetCore.Mvc;
using DancingGoat.Infrastructure;
using System.Globalization;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    [TypeFilter(typeof(SelfConfigActionFilterAttribute))]
    public class ControllerBase : Controller
    {
        protected readonly IDeliveryClient _client;
        protected string Language => CultureInfo.CurrentCulture.Name;
        public ControllerBase(IDeliveryClientFactory deliveryClientFactory)
        {
            _client = deliveryClientFactory.Get();
        }
    }
}

