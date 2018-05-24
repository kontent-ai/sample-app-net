using System;
using System.Globalization;
using System.Web.Mvc;

using DancingGoat.Helpers;
using DancingGoat.Localization;
using KenticoCloud.Delivery;

namespace DancingGoat.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected readonly DeliveryClient baseClient;
        public readonly IDeliveryClient client;

        public ControllerBase()
        {
            baseClient = DeliveryClientFactory.CreateDeliveryClient();
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            if (currentCulture.Equals(LanguageClient.DEFAULT_LANGUAGE, StringComparison.InvariantCultureIgnoreCase))
            {
                client = baseClient;
            }
            else
            {
                client = new LanguageClient(baseClient, currentCulture);
            }
        }
    }
}

