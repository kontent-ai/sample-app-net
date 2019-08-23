using DancingGoat.Models;
using KenticoCloud.Delivery;
using DancingGoat.Areas.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DancingGoat.Controllers
{
    // TODO: Is this a service or what????
    public class ControllerBase : Controller
    {
        private readonly DeliveryOptions _deliveryOptions;
        protected readonly IDeliveryClient _client;
        private readonly AppSettingProvider _settingProvider;

        public ControllerBase(IOptionsSnapshot<DeliveryOptions> deliveryOptions
            , AppSettingProvider settingProvider)
        {
            // TODO: Add localization, see https://github.com/Kentico/cloud-sample-app-net/blob/ae6cb700d1d8d08b6e8141403c6198a796c9c2bc/DancingGoat/Controllers/ControllerBase.cs#L20-L28
            _deliveryOptions = deliveryOptions.Value;
            _settingProvider = settingProvider;
            _client = this.CreateDeliveryClient();
        }


        public IDeliveryClient CreateDeliveryClient()
        {
            // Build DeliveryOptions with default or explicit values

            var projectId = _deliveryOptions.ProjectId ?? _settingProvider.ProjectId?.ToString();

            var clientInstance = DeliveryClientBuilder.WithOptions(o => _deliveryOptions)
                .WithTypeProvider(new CustomTypeProvider())
                .WithContentLinkUrlResolver(new CustomContentLinkUrlResolver()).Build();

            return clientInstance;
        }
    }
}

