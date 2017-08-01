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