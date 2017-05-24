using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DancingGoat.Infrastructure
{
    public class SelfConfigActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            DateTime? subscriptionExpiresAt = Areas.Admin.AppSettingProvider.SubscriptionExpiresAt;

            if ((filterContext.HttpContext.Request.HttpMethod == "GET" && !filterContext.ActionParameters.Any()))
            {
                if (subscriptionExpiresAt == DateTime.MinValue)
                {
                    filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigRedirectResult(null);
                }
                else if (subscriptionExpiresAt <= DateTime.Now)
                {
                    filterContext.Result = Helpers.RedirectHelpers.GetSelfConfigRecheckResult(null);
                } 
            }
        }
    }
}