using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        //protected const string KC_BASE_URL = @"https://app.kenticocloud.com/api/";
        protected const string KC_BASE_URL = @"https://kenticolabs-cdn-develop.azurewebsites.net/api/";
        protected const string MESSAGE_UNAUTHENTICATED = "You haven't successfully authenticated with Kentico Cloud credentials. Please check your credentials and log in.";
        protected const string SHARED_PROJECT_ID = "975bf280-fd91-488c-994c-2f04416e5ee3";

        protected readonly HttpClient _httpClient = new HttpClient();

        [HttpGet]
        public ActionResult Index()
        {
            //ViewBag.Url = HttpUtility.UrlEncode(Request.Url.ToString());
            ViewBag.IsTls = Request.IsSecureConnection;
            ViewBag.IsLocal = Request.IsLocal;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string token)
        {
            //if (!string.IsNullOrEmpty(Request.Headers["X-Auth"]))
            if (!string.IsNullOrEmpty(token))
            {
                Models.SubscriptionModel subscription;

                try
                {
                    subscription = await GetSubscriptionAsync(token);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // No need to work with 'ex'.
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRedirectResult(MESSAGE_UNAUTHENTICATED);
                }

                if (subscription == null)
                {
                    Guid projectGuid = await StartTrial(token);

                    if (projectGuid != Guid.Empty)
                    {
                        AppSettingProvider.ProjectId = projectGuid;
                        AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                        // Fire-and-forget. Assigned to 'task' just to avoid warnings.
                        var task = RenameProject(projectGuid);

                        return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(null);
                    }
                    else
                    {
                        return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"There was an error when setting the 'ProjectId' app setting to '{projectGuid}'. Please set it up manually.");
                    }
                }
                else
                {
                    ActionResult result = CheckInactiveSubscriptions(token, subscription);

                    // Active non-expired subscription. Will deploy a sample project.
                    if (result == null && subscription.Status == "active" && (subscription.EndAt == null || subscription.EndAt > DateTime.Now))
                    {
                        Guid projectGuid = await DeploySample(token);

                        if (projectGuid != Guid.Empty)
                        {
                            AppSettingProvider.ProjectId = projectGuid;
                            AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                            // Fire-and-forget. Assigned to 'task' just to avoid warnings.
                            var task = RenameProject(projectGuid);

                            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(null);
                        }
                        else
                        {
                            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("There was an error when deploying the sample project");
                        }
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRedirectResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpGet]
        public ActionResult Recheck()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Recheck(string token)
        {
            //if (!string.IsNullOrEmpty(Request.Headers["X-Auth"]))
            if (!string.IsNullOrEmpty(token))
            {
                Models.SubscriptionModel subscription;

                try
                {
                    subscription = await GetSubscriptionAsync(token);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // No need to work with 'ex'.
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRedirectResult(MESSAGE_UNAUTHENTICATED);
                }

                if (subscription.SubscriptionId.HasValue)
                {
                    return CheckInactiveSubscriptions(token, subscription);
                }
                else
                {
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult("There was an error when getting the subscription data.");
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpGet]
        public ActionResult Expired(string token, DateTime? endAt)
        {
            return View(new Models.ExpiredViewModel { EndAt = endAt, Token = token });
        }

        [HttpPost]
        public async Task<ActionResult> Free(string token)
        {
            bool isAuthenticated = false;
            isAuthenticated = await ChechAuthentication(token);

            if (isAuthenticated)
            {
                Models.SubscriptionModel subscription;

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}subscription/free"))
                using (HttpResponseMessage response = await GetStandardResponse(token, request))
                {
                    subscription = await GetResult<Models.SubscriptionModel>(response);

                    if (subscription != null & subscription.PlanName == "free")
                    {
                        AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                        return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've converted your subscription to a free plan.");
                    }
                    else
                    {
                        return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("We couldn't convert your subscription to a free plan. Make sure you've lowered your resources in Kentico Cloud to meet the limits of the free plan.");
                    }
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRedirectResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpPost]
        public ActionResult ProjectId(Models.ProjectIdUpViewModel model)
        {
            Guid ownProjectId;

            if (Guid.TryParse(model.OwnProjectGuid, out ownProjectId))
            {
                AppSettingProvider.ProjectId = ownProjectId;
                AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"You've configured your app with a Project ID '{model.OwnProjectGuid}'.");
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"There was an error configuring the Project ID. Make sure the ID has the form of a GUID and try again.");
            }
        }

        [HttpPost]
        public ActionResult Shared()
        {
            AppSettingProvider.ProjectId = Guid.Parse(SHARED_PROJECT_ID);
            AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've pointed your app to a shared Kentico Cloud project.");
        }

        [HttpGet]
        public ActionResult Activate(string token)
        {
            return View(token);
        }

        private Task RenameProject(Guid projectGuid)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> StartTrial(string token)
        {

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}subscription/trial"))
            using (HttpResponseMessage response = await GetStandardResponse(token, request))
            {
                var subscription = await GetResult<Models.SubscriptionModel>(response);

                if (subscription != null & subscription.SubscriptionId.HasValue)
                {
                    return await DeploySample(token);
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        private async Task<Guid> DeploySample(string token)
        {
            bool sampleReady = false;
            Models.ProjectModel project;

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}project/sample"))
            using (HttpResponseMessage response = await GetStandardResponse(token, request))
            {
                project = await GetResult<Models.ProjectModel>(response);

                if (project != null & project.ProjectGuid.HasValue)
                {
                    sampleReady = true;
                }
            }

            return sampleReady ? project.ProjectGuid.Value : Guid.Empty;
        }

        private async Task<TResult> GetResult<TResult>(HttpResponseMessage response)
            where TResult : class
        {
            TResult result = null;

            try
            {
                result = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return null;
            }

            return result;
        }

        private async Task<HttpResponseMessage> GetStandardResponse(string token, HttpRequestMessage request)
        {
            AddStandardHeaders(token, request);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            return response;
        }


        private ActionResult CheckInactiveSubscriptions(string token, Models.SubscriptionModel subscription)
        {
            // Inactive expired subscription. Will be offered to either convert to the free plan or to use shared project.
            if (subscription.Status != "active" && subscription.EndAt <= DateTime.Now)
            {
                return RedirectToAction("Expired", new { Token = token, EndAt = subscription.EndAt });
            }

            // Inactive not-expired subscription. Will be offered to activate it (manually) and continue.
            else if (subscription.Status != "active" && subscription.EndAt > DateTime.Now)
            {
                return RedirectToAction("Activate", new { Token = token });
            }

            return null;
        }

        private async Task<Models.SubscriptionModel> GetSubscriptionAsync(string token)
        {
            bool isAuthenticated = false;
            isAuthenticated = await ChechAuthentication(token);

            if (isAuthenticated)
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}subscription"))
                using (HttpResponseMessage response = await GetStandardResponse(token, request))
                {
                    try
                    {
                        var responseAsString = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<IEnumerable<Models.SubscriptionModel>>(responseAsString).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
            }
            else
            {
                // HACK Avoid throwing in a private method.
                throw new UnauthorizedAccessException();
            }
        }

        private async Task<bool> ChechAuthentication(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}auth"))
            using (HttpResponseMessage response = await GetStandardResponse(token, request))
            {
                return response.IsSuccessStatusCode;
            }
        }

        private static void AddStandardHeaders(string token, HttpRequestMessage request)
        {
            request.Headers.Add("X-Auth", token);
        }
    }
}