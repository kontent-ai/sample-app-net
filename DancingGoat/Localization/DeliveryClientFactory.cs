using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;

namespace DancingGoat.Localization
{
    public class DeliveryClientFactory : IDeliveryClientFactory
    {
        public DeliveryOptions Options { get; }

        public DeliveryClientFactory(IOptionsSnapshot<DeliveryOptions> options)
        {
            Options = options.Value;
        }

        public IDeliveryClient GetDeliveryClient()
        {
            var baseClient = DeliveryClientBuilder.WithOptions(o => Options)
                   .WithTypeProvider(new CustomTypeProvider())
                   .WithContentLinkUrlResolver(new CustomContentLinkUrlResolver()).Build();

            var currentCulture = CultureInfo.CurrentUICulture.Name;
            if (currentCulture.Equals(LanguageClient.DEFAULT_LANGUAGE, StringComparison.InvariantCultureIgnoreCase))
            {                
                return baseClient;
            }
            else
            {
                return new LanguageClient(baseClient, currentCulture);
            }
        }
    }
}
