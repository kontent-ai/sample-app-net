using Microsoft.AspNetCore.Mvc;
using DancingGoat.Infrastructure;
using System.Globalization;
using Kontent.Ai.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    [TypeFilter(typeof(SelfConfigActionFilterAttribute))]
    [TypeFilter(typeof(CultureValidatorActionFilter))]
    public class ControllerBase : Controller
    {
        protected readonly IDeliveryClient _client;

        protected string Language => CultureInfo.CurrentUICulture.Name;

        public ControllerBase(IDeliveryClientFactory deliveryClientFactory) : this()
        {
            _client = deliveryClientFactory.Get();
        }

        public ControllerBase()
        {

        }
    }
}

