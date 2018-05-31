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
        protected readonly IDeliveryClient baseClient = new SampleDeliveryClient();
        protected readonly IDeliveryClient client;

        public ControllerBase()
        {
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

