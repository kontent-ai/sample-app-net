using System;
using System.Linq;
using System.Web.Mvc;

namespace DancingGoat.Infrastructure
{
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            DateTime? subscriptionExpiresAt = Areas.Admin.AppSettingProvider.SubscriptionExpiresAt;

            // TODO Make it work without heavy checks below.
            if ((filterContext.HttpContext.Request.HttpMethod == "GET" && !filterContext.ActionParameters.Any()))
            {
                if (subscriptionExpiresAt == DateTime.MinValue)
                {
                    filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigIndexResult(null);
                }
                else if (subscriptionExpiresAt <= DateTime.Now)
                {
                    filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigRecheckResult(null);
                } 
            }
        }
    }
}