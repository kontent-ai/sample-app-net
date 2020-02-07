using System;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Areas.Admin.Models;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace DancingGoat.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {
        public IOptions<AppConfiguration> Config { get; }
        public IOptions<DeliveryOptions> DeliveryOptions { get; }

        public SelfConfigActionFilterAttribute(IOptions<AppConfiguration> config, IOptions<DeliveryOptions> deliveryOptions)
        {
            Config = config;
            DeliveryOptions = deliveryOptions;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (string.IsNullOrEmpty(DeliveryOptions.Value.ProjectId))
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(null);
            }
            else if (Config.Value.SubscriptionExpiresAt.HasValue && Config.Value.SubscriptionExpiresAt <= DateTime.Now)
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(new MessageModel(){ Message = "Current subscription is expired.", MessageType = Areas.Admin.Models.MessageType.Error});
            }
        }
    }
}