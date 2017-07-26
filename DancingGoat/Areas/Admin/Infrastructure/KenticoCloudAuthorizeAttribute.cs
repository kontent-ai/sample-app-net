using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DancingGoat.Areas.Admin.Helpers;
using System.Threading.Tasks;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class KenticoCloudAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string token = null;

            if (string.IsNullOrEmpty(filterContext.HttpContext.Request.Cookies["kcToken"]?.Value))
            {
                filterContext.Result = GetResult();
            }
            else
            {
                token = filterContext.HttpContext.Request.Cookies["kcToken"].Value;
            }

            var user = Task.Run(() => AdminHelpers.GetUserAsync(token, new System.Net.Http.HttpClient(), AdminHelpers.KC_BASE_URL)).Result;

            if (user == null)
            {
                filterContext.Result = GetResult();
            }
        }

        private ActionResult GetResult()
        {
            DateTime? subscriptionExpiresAt = AppSettingProvider.SubscriptionExpiresAt;
            string message = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.";

            if (subscriptionExpiresAt == DateTime.MinValue)
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(message);
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult(message);
            }
        }
    }
}