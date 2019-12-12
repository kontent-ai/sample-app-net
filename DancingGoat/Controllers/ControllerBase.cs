using DancingGoat.Models;
using DancingGoat.Areas.Admin.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using DancingGoat.Infrastructure;
using DancingGoat.Areas.Admin;
using Microsoft.Extensions.Options;

namespace DancingGoat.Controllers
{
    [TypeFilter(typeof(SelfConfigActionFilterAttribute))]
    public class ControllerBase : Controller
    {
        private readonly DeliveryOptions _deliveryOptions;
        protected readonly IDeliveryClient _client;
        protected readonly IAppSettingProvider _settingProvider;

        public ControllerBase(IOptionsSnapshot<DeliveryOptions> deliveryOptions
            , IAppSettingProvider settingProvider, IDeliveryClient deliveryClient)
        {
            // TODO: Add localization, see https://github.com/Kentico/cloud-sample-app-net/blob/ae6cb700d1d8d08b6e8141403c6198a796c9c2bc/DancingGoat/Controllers/ControllerBase.cs#L20-L28
            _deliveryOptions = deliveryOptions.Value;
            _settingProvider = settingProvider;
            _client = deliveryClient = this.CreateDeliveryClient();
        }


        public IDeliveryClient CreateDeliveryClient()
        {
            // Build DeliveryOptions with default or explicit values

            _deliveryOptions.ProjectId = _deliveryOptions.ProjectId ?? _settingProvider.GetProjectId()?.ToString();

            var clientInstance = DeliveryClientBuilder.WithOptions(o => _deliveryOptions)
                .WithTypeProvider(new CustomTypeProvider())
                .WithContentLinkUrlResolver(new CustomContentLinkUrlResolver()).Build();

            return clientInstance;
        }
    }
}

