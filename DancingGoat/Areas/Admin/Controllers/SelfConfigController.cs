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
        protected const string MESSAGE_GENERAL_ERROR = "An unknown error occurred.";
        protected const string SHARED_PROJECT_ID = "975bf280-fd91-488c-994c-2f04416e5ee3";
        protected const string STATUS_ACTIVE = "active";

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
                IEnumerable<Models.SubscriptionModel> subscriptions;

                try
                {
                    subscriptions = await GetSubscriptionsAsync(token);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // No need to work with 'ex'.
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
                }

                if (subscriptions == null || !subscriptions.Any())
                {
                    Guid projectId = await StartTrialAndSampleAsync(token);

                    return SetProject(projectId, DateTime.Now.AddDays(30));
                }
                else
                {
                    var activeProjects = await GetActiveProjectsAsync(token);
                    ActionResult result = await CheckInactiveSubscriptions(token, subscriptions, activeProjects);

                    if (result == null)
                    {
                        if (activeProjects.Any())
                        {
                            return View("ChooseOrDeployProject", activeProjects);
                        }
                        else
                        {
                            Guid projectId = await DeploySampleAsync(token);

                            return SetProject(projectId, DateTime.MaxValue);
                        } 
                    }

                    // A formal line of code to satisfy the compiler check. The "result" is either null or the MVC pipeline got shortcut already (an ActionResult was returned to the browser).
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigErrorResult(MESSAGE_GENERAL_ERROR);
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Recheck(string token)
        {
            //if (!string.IsNullOrEmpty(Request.Headers["X-Auth"]))
            if (!string.IsNullOrEmpty(token))
            {
                IEnumerable<Models.SubscriptionModel> subscriptions;

                try
                {
                    subscriptions = await GetSubscriptionsAsync(token);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // No need to work with 'ex'.
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
                }

                if (subscriptions == null || !subscriptions.Any())
                {
                    return View("NoSubscriptions");
                }
                else
                {
                    var activeProjects = await GetActiveProjectsAsync(token);

                    // The trial period expired, but the web job hasn't converted the trial to the free plan yet.
                    ActionResult result = await CheckInactiveSubscriptions(token, subscriptions, activeProjects);

                    // The trial was converted to the free plan by the web job.
                    if (result == null)
                    {
                        if (activeProjects.Any(p => p.ProjectGuid == AppSettingProvider.ProjectId))
                        {
                            AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(null);
                        }
                        else
                        {
                            return View("ChooseOrDeployProject", activeProjects);
                        }
                    }

                    // A formal line of code to satisfy the compiler check. The "result" is either null or the MVC pipeline got shortcut already (an ActionResult was returned to the browser).
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigErrorResult(MESSAGE_GENERAL_ERROR);
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigErrorResult(MESSAGE_UNAUTHENTICATED);
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
            isAuthenticated = await ChechAuthenticationAsync(token);

            if (isAuthenticated)
            {
                Models.SubscriptionModel subscription;

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}subscription/free"))
                using (HttpResponseMessage response = await GetStandardResponseAsync(token, request))
                {
                    subscription = await GetResultAsync<Models.SubscriptionModel>(response);

                    if (subscription != null && subscription.PlanName == "free")
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
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpPost]
        public ActionResult SetProjectId(Models.ProjectIdViewModel model)
        {
            Guid ownProjectId;

            if (Guid.TryParse(model.OwnProjectGuid, out ownProjectId))
            {
                AppSettingProvider.ProjectId = ownProjectId;
                AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"You've configured your app with a Project ID \"{model.OwnProjectGuid}\".");
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"There was an error configuring the Project ID. Make sure the ID has the form of a GUID and try again.");
            }
        }

        [HttpPost]
        public ActionResult UseShared()
        {
            AppSettingProvider.ProjectId = Guid.Parse(SHARED_PROJECT_ID);
            AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've configured your app to with a Project ID of a shared Kentico Cloud project.");
        }

        [HttpGet]
        public ActionResult Activate(string token)
        {
            return View(token);
        }

        [HttpGet]
        public async Task<ActionResult> ActiveProjects(string token)
        {
            return View((await GetActiveProjectsAsync(token)).Where(p => p.Inactive == false));
        }

        [HttpPost]
        public async Task<ActionResult> DeploySample(string token)
        {
            Guid projectId = await DeploySampleAsync(token);

            return SetProject(projectId, DateTime.MaxValue);
        }

        private async Task<ActionResult> CheckInactiveSubscriptions(string token, IEnumerable<Models.SubscriptionModel> subscriptions = null, IEnumerable<Models.ProjectModel> projects = null)
        {
            var activeProjects = projects?.Where(p => p.Inactive == false) ?? await GetActiveProjectsAsync(token);
            var allSubscriptions = subscriptions ?? await GetSubscriptionsAsync(token);
            var activeSubscriptions = allSubscriptions?.Where(s => s.Status.Equals(STATUS_ACTIVE, StringComparison.InvariantCultureIgnoreCase));

            // Inactive expired subscriptions. 
            if (!activeSubscriptions.Any() && !allSubscriptions.Any(s => s.EndAt > DateTime.Now))
            {
                var subscriptionsExpiringInFuture = allSubscriptions.Where(s => s.EndAt > DateTime.Now).OrderByDescending(s => s.EndAt);

                // Will allow to either convert to the free plan or to use shared project.
                if (!activeProjects.Any())
                {
                    return RedirectToAction("Expired", new { Token = token, EndAt = subscriptionsExpiringInFuture.FirstOrDefault().EndAt });
                }

                // Will allow to do the same, and to pick among active projects.
                else
                {
                    return RedirectToAction("ExpiredSelectProject", new { Token = token, EndAt = subscriptionsExpiringInFuture.FirstOrDefault().EndAt, Projects = activeProjects });
                }
            }

            // Inactive non-expired subscriptions.
            else if (!activeSubscriptions.Any() && allSubscriptions.Any(s => s.EndAt > DateTime.Now))
            {
                // Will suggest to activate one of them (manually) and continue.
                if (!activeProjects.Any())
                {
                    return RedirectToAction("Activate", new { Token = token });
                }

                // Will suggest to do the same, alternatively allow to pick among active projects.
                else
                {
                    return RedirectToAction("ActivateSelectProject", new { Token = token, Projects = activeProjects });
                }
            }

            return null;
        }

        private ActionResult SetProject(Guid projectId, DateTime subscriptionExpiresAt, string message = null)
        {
            if (projectId != Guid.Empty)
            {
                SetIdAndRename(projectId, subscriptionExpiresAt);

                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(message);
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigErrorResult($"There was a problem when setting the \"ProjectId\" environment variable to \"{projectId}\". You can set it up manually in your environment.");
            }
        }

        private void SetIdAndRename(Guid projectId, DateTime subscriptionExpiresAt)
        {
            AppSettingProvider.ProjectId = projectId;
            AppSettingProvider.SubscriptionExpiresAt = subscriptionExpiresAt;

            // Fire-and-forget. Assigned to 'task' just to avoid warnings.
            var task = RenameProjectAsync(projectId);
        }

        private Task RenameProjectAsync(Guid projectGuid)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> StartTrialAndSampleAsync(string token)
        {

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}subscription/trial"))
            using (HttpResponseMessage response = await GetStandardResponseAsync(token, request))
            {
                var subscription = await GetResultAsync<Models.SubscriptionModel>(response);

                if (subscription != null && subscription.SubscriptionId.HasValue)
                {
                    return await DeploySampleAsync(token);
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }

        private async Task<Guid> DeploySampleAsync(string token)
        {
            bool sampleReady = false;
            Models.ProjectModel project;

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}project/sample"))
            using (HttpResponseMessage response = await GetStandardResponseAsync(token, request))
            {
                project = await GetResultAsync<Models.ProjectModel>(response);

                if (project != null && project.ProjectGuid.HasValue)
                {
                    sampleReady = true;
                }
            }

            return sampleReady ? project.ProjectGuid.Value : Guid.Empty;
        }

        private async Task<IEnumerable<Models.ProjectModel>> GetActiveProjectsAsync(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}project"))
            using (HttpResponseMessage response = await GetStandardResponseAsync(token, request))
            {
                return (await GetResultAsync<IEnumerable<Models.ProjectModel>>(response)).Where(p => p.Inactive == false);
            }
        }

        private async Task<TResult> GetResultAsync<TResult>(HttpResponseMessage response)
            where TResult : class
        {
            TResult result = null;

            // TODO Deal with it upper in the call stack.
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

        private async Task<HttpResponseMessage> GetStandardResponseAsync(string token, HttpRequestMessage request)
        {
            AddStandardHeaders(token, request);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            return response;
        }

        private async Task<IEnumerable<Models.SubscriptionModel>> GetSubscriptionsAsync(string token)
        {
            bool isAuthenticated = false;
            isAuthenticated = await ChechAuthenticationAsync(token);

            if (isAuthenticated)
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}subscription"))
                using (HttpResponseMessage response = await GetStandardResponseAsync(token, request))
                {
                    return await GetResultAsync<IEnumerable<Models.SubscriptionModel>>(response);
                }
            }
            else
            {
                // HACK Avoid throwing in a private method.
                throw new UnauthorizedAccessException();
            }
        }

        private async Task<bool> ChechAuthenticationAsync(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}auth"))
            using (HttpResponseMessage response = await GetStandardResponseAsync(token, request))
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