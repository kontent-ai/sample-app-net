using DancingGoat.Areas.Admin;
using DancingGoat.Infrastructure;
using DancingGoat.Localization;
using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using System;
using System.Globalization;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [SelfConfigActionFilter]
    public class ControllerBase : AsyncController
    {
        protected static readonly IDeliveryClient baseClient = CreateDeliveryClient();
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


        public static IDeliveryClient CreateDeliveryClient()
        {
            // Use the provider to get environment variables
            ConfigurationManagerProvider provider = new ConfigurationManagerProvider();

            // Build DeliveryOptions with default or explicit values
            var options = provider.GetDeliveryOptions();

            options.ProjectId = options.ProjectId ?? AppSettingProvider.DefaultProjectId.ToString();
            var clientInstance = DeliveryClientBuilder.WithOptions(o => options)
                .WithTypeProvider(new CustomTypeProvider())
                .WithContentLinkUrlResolver(new CustomContentLinkUrlResolver()).Build();

            return clientInstance;
        }
    }
}

