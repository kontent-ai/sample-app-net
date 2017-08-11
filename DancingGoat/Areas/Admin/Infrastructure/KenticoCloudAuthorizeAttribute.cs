using System;
using System.Web.Mvc;

using System.Threading.Tasks;
using System.Net.Http;
using DancingGoat.Areas.Admin.Models;
using DancingGoat.Helpers;

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
                var message = new MessageModel { Caption = null, Message = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.", MessageType = MessageType.Error };
                filterContext.Result = subscriptionExpiresAt == null ? RedirectHelpers.GetSelfConfigIndexResult(message) : RedirectHelpers.GetSelfConfigRecheckResult(message);
            }
        }
    }
}