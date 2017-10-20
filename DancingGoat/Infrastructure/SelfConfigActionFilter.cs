using System;
using System.Web.Mvc;

namespace DancingGoat.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            DateTime? subscriptionExpiresAt = Areas.Admin.AppSettingProvider.SubscriptionExpiresAt;
            Guid? projectId = Areas.Admin.AppSettingProvider.ProjectId;

            if (!projectId.HasValue)
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(null);
            }
            else if (subscriptionExpiresAt.HasValue && subscriptionExpiresAt <= DateTime.Now)
            {
                filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigRecheckResult(null);
            }
        }
    }
}