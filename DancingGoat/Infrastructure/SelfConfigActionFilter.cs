using System;
using DancingGoat.Areas.Admin;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DancingGoat.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {
        private readonly AppSettingProvider _settingProvider;

        public SelfConfigActionFilterAttribute(IAppSettingProvider settingProvider)
        {
            _settingProvider = settingProvider as AppSettingProvider;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            DateTime? subscriptionExpiresAt =_settingProvider.SubscriptionExpiresAt;
            Guid? projectId = _settingProvider.ProjectId;

            if (!projectId.HasValue)
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(null);
            }
            else if (subscriptionExpiresAt.HasValue && subscriptionExpiresAt <= DateTime.Now)
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(new MessageModel(){ Message = "Current subscription is expired.", MessageType = Areas.Admin.Models.MessageType.Error});
            }
        }
    }
}