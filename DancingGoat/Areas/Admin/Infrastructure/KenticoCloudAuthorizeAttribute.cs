using System;
using System.Web.Mvc;

using System.Threading.Tasks;
using System.Net.Http;
using DancingGoat.Areas.Admin.Models;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class KenticoCloudAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var token = filterContext.HttpContext.Request.Cookies["kcToken"]?.Value;
            UserModel user = null;
            
            if (!string.IsNullOrEmpty(token))
            {
                user = Task.Run(() => (new UserProvider(new HttpClient())).GetUserAsync(token)).Result;
            }

            if (user == null)
            {
                DateTime? subscriptionExpiresAt = AppSettingProvider.SubscriptionExpiresAt;
                string message = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.";
                filterContext.Result = subscriptionExpiresAt == DateTime.MinValue ?
                    DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(message) :
                    DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult(message);
            }
        }
    }
}