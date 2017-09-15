using System;
using System.Globalization;
using System.Web.Mvc;

using KenticoCloud.Delivery;

using DancingGoat.Areas.Admin;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using DancingGoat.Localization;

namespace DancingGoat.Controllers
{
    [SelfConfigActionFilter]
    public class ControllerBase : AsyncController
    {
        protected static readonly DeliveryClient baseClient = CreateDeliveryClient();
        public readonly IDeliveryClient client;

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
       

        public static DeliveryClient CreateDeliveryClient()
        {
            // Use the provider to get environment variables.
            var provider = new ConfigurationManagerProvider();

            // Build DeliveryOptions with default or explicit values.
            var options = provider.GetDeliveryOptions();

            options.ProjectId = options.ProjectId ?? AppSettingProvider.DefaultProjectId.ToString();
            var clientInstance = new DeliveryClient(options);
            clientInstance.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            clientInstance.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
            return clientInstance;
        }
    }
}

