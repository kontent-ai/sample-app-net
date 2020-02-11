using System;
using DancingGoat.Areas.Admin.Models;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace DancingGoat.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {
        public IOptionsSnapshot<AppConfiguration> AppConfig { get; }

        public IOptionsSnapshot<DeliveryOptions> DeliveryOptions { get; }

        public SelfConfigActionFilterAttribute(IOptionsSnapshot<AppConfiguration> appConfig, IOptionsSnapshot<DeliveryOptions> deliveryOptions)
        {
            AppConfig = appConfig;
            DeliveryOptions = deliveryOptions;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (string.IsNullOrEmpty(DeliveryOptions.Value.ProjectId))
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(null);
            }
            else if (AppConfig.Value.SubscriptionExpiresAt.HasValue && AppConfig.Value.SubscriptionExpiresAt <= DateTime.Now)
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(new MessageModel(){ Message = "Current subscription is expired.", MessageType = Areas.Admin.Models.MessageType.Error});
            }
        }
    }
}