using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using DancingGoat.Infrastructure;
using System.Globalization;

namespace DancingGoat.Controllers
{
    [TypeFilter(typeof(SelfConfigActionFilterAttribute))]
    public class ControllerBase : Controller
    {
        protected readonly IDeliveryClient _client;
        protected string Language => CultureInfo.CurrentCulture.Name;
        public ControllerBase(IDeliveryClient deliveryClient)
        {
            _client = deliveryClient;
        }
    }
}

