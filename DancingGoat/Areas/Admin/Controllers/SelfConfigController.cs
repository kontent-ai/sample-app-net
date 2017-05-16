using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        protected const string KC_BASE_URL = @"https://app.kenticocloud.com/api/";
        protected const string MESSAGE_UNAUTHENTICATED = "You haven't successfully authenticated with Kentico Cloud credentials.";

        // GET: Admin/SelfConfig
        public ActionResult Index()
        {
            return View();
        }

        // POST: Admin/SelfConfig/Process
        [HttpPost]
        public ActionResult Index(string token)
        {
            //if (!string.IsNullOrEmpty(Request.Headers["X-Auth"]))
            if (!string.IsNullOrEmpty(token))
            {

            }
            else
            {
                return Helpers.RedirectHelpers.GetSelfConfigRedirectResult();
            }
        }

        // GET: Admin/SelfConfig/Recheck
        public ActionResult Recheck()
        {
            return View();
        }

        // POST: Admin/SelfConfig/Recheck
        public ActionResult Recheck(string token)
        {
            //if (!string.IsNullOrEmpty(Request.Headers["X-Auth"]))
            if (!string.IsNullOrEmpty(token))
            {

            }
            else
            {
                return Helpers.RedirectHelpers.GetSelfConfigRecheckResult();
            }
        }

        private async Task<ActionResult> CheckSubscriptionAsync(bool recheck)
        {
            using (HttpClient client = new HttpClient())
            {
                bool isAuthenticated = false;

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}subscription"))
                {
                    HttpResponseMessage response = await client.SendAsync(request);
                    isAuthenticated = response.IsSuccessStatusCode;
                }

                if (isAuthenticated)
                {

                }
                else
                {
                    return recheck ? Helpers.RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_UNAUTHENTICATED) : Helpers.RedirectHelpers.GetSelfConfigRedirectResult(MESSAGE_UNAUTHENTICATED);
                }
            }
        }
    }
}