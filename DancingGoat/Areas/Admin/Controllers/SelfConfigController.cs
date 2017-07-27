using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using DancingGoat.Areas.Admin.Infrastructure;
using DancingGoat.Areas.Admin.Helpers;
using DancingGoat.Areas.Admin.Models;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        protected const string MESSAGE_UNAUTHENTICATED = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.";
        protected const string SHARED_PROJECT_ID = "975bf280-fd91-488c-994c-2f04416e5ee3";
        protected const string STATUS_ACTIVE = "active";
        protected const string PROJECT_RENAME_PATTERN = "Sample Project (MVC Sample App, {0})";
        protected const string COOKIE_NAME = "kcToken";

        protected readonly HttpClient _httpClient = new HttpClient();

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IsTls = Request.IsSecureConnection;
            ViewBag.IsLocal = Request.IsLocal;

            return View(new IndexViewModel { SignInModel = new SignInViewModel() });
        }

        [HttpPost]
        public async Task<ActionResult> Index(string token)
        {
            string actualToken = token ?? GetToken();

            if (!string.IsNullOrEmpty(actualToken))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    AddCookie(token);
                }

                var user = await AdminHelpers.GetUserAsync(actualToken, _httpClient, AdminHelpers.KC_BASE_URL);

                IEnumerable<SubscriptionModel> subscriptions;

                if (user != null)
                {
                    subscriptions = await GetSubscriptionsAsync(actualToken);
                }
                else
                {
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
                }

                if (subscriptions == null || !subscriptions.Any())
                {
                    var subscriptionAndProject = await StartTrialAndSampleAsync(actualToken);

                    if (subscriptionAndProject.subscription != null && subscriptionAndProject.project != null)
                    {
                        return SetProject(actualToken, subscriptionAndProject.project.ProjectId.Value, subscriptionAndProject.subscription.EndAt.Value);
                    }
                    else
                    {
                        return SetSharedProject(actualToken, "There was an error when creating the sample site.");
                    }
                }
                else
                {
                    var activeProjects = await GetProjectsAsync(actualToken);
                    ActionResult result = await CheckInactiveSubscriptions(actualToken, subscriptions, activeProjects);

                    if (result == null)
                    {
                        if (activeProjects.Any())
                        {
                            return View("SelectOrCreateProject", BuildSelectProjectViewModel(activeProjects));
                        }
                        else
                        {
                            return await DeploySampleInOwnedSubscriptionAsync(actualToken, user, subscriptions);
                        }
                    }

                    return result;
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpGet]
        public ActionResult Recheck()
        {
            ViewBag.IsTls = Request.IsSecureConnection;
            ViewBag.IsLocal = Request.IsLocal;

            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Recheck(string token)
        {
            string actualToken = token ?? GetToken();

            if (!string.IsNullOrEmpty(actualToken))
            {
                if (!string.IsNullOrEmpty(token))
                {
                    AddCookie(token);
                }

                IEnumerable<SubscriptionModel> subscriptions;

                try
                {
                    subscriptions = await GetSubscriptionsAsync(actualToken);
                }
                catch (UnauthorizedAccessException)
                {
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_UNAUTHENTICATED);
                }

                if (subscriptions == null || !subscriptions.Any())
                {
                    return View("NoSubscriptions");
                }
                else
                {
                    var activeProjects = await GetProjectsAsync(actualToken);

                    // The trial period expired, but the web job hasn't converted the trial to the free plan yet.
                    ActionResult result = await CheckInactiveSubscriptions(actualToken, subscriptions, activeProjects);

                    // The trial was converted to the free plan by the web job.
                    if (result == null)
                    {
                        if (activeProjects.Any(p => p.ProjectId == AppSettingProvider.ProjectId))
                        {
                            AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(null);
                        }
                        else
                        {
                            return View("SelectOrCreateProject", BuildSelectProjectViewModel(activeProjects));
                        }
                    }

                    return result;
                }
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> Free()
        {
            string token = GetToken();

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{AdminHelpers.KC_BASE_URL}subscription/free"))
            {
                using (HttpResponseMessage response = await AdminHelpers.GetStandardResponseAsync(token, _httpClient, request))
                {
                    var subscription = await AdminHelpers.GetResultAsync<SubscriptionModel>(response);

                    if (subscription != null && subscription.PlanName == "free")
                    {
                        AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                        return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've converted your subscription to a free plan.");
                    }
                    else
                    {
                        return View("Error", new ErrorModel { Caption = "Too Many Resources", Message = "We couldn't convert your subscription to the free plan. Please make sure you've lowered your resources in Kentico Cloud to meet the limits of the free plan." });
                    }
                }
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public ActionResult SelectProject(SelectProjectViewModel model)
        {
            string token = GetToken();

            if (model.ProjectId != Guid.Empty)
            {
                AppSettingProvider.ProjectId = model.ProjectId;
                AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"You've configured your app with a Project ID \"{model.ProjectId}\".");
            }
            else
            {
                return SetSharedProject(token, "The Project ID could not be found.");
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public ActionResult UseShared()
        {
            AppSettingProvider.ProjectId = Guid.Parse(SHARED_PROJECT_ID);
            AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;

            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've configured your app to with a Project ID of a shared Kentico Cloud project.");
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> DeploySample()
        {
            string token = GetToken();
            var user = await AdminHelpers.GetUserAsync(token, _httpClient, AdminHelpers.KC_BASE_URL);
            var subscriptions = await GetSubscriptionsAsync(token);
            return await DeploySampleInOwnedSubscriptionAsync(token, user, subscriptions);
        }

        private async Task<ActionResult> CheckInactiveSubscriptions(string token, IEnumerable<SubscriptionModel> subscriptions = null, IEnumerable<ProjectModel> projects = null)
        {
            var activeProjects = projects?.Where(p => p.Inactive == false) ?? await GetProjectsAsync(token);
            var allSubscriptions = subscriptions ?? await GetSubscriptionsAsync(token);
            var activeSubscriptions = allSubscriptions?.Where(s => s.Status.Equals(STATUS_ACTIVE, StringComparison.InvariantCultureIgnoreCase));

            // Inactive expired subscriptions. 
            if (!activeSubscriptions.Any() && !allSubscriptions.All(s => s.EndAt > DateTime.Now))
            {
                var latestExpiredSubscriptions = allSubscriptions.OrderByDescending(s => s.EndAt);

                // Will allow to either convert to the free plan or to use the shared project.
                if (!activeProjects.Any())
                {
                    ViewBag.EndAt = latestExpiredSubscriptions.FirstOrDefault().EndAt.Value;

                    // Project = null means the project selection won't be displayed.
                    return View("Expired", new SelectProjectViewModel { Projects = null });
                }

                // Will allow to do the same, and to pick among active projects.
                else
                {
                    ViewBag.EndAt = latestExpiredSubscriptions.FirstOrDefault().EndAt.Value;

                    return View("Expired", new SelectProjectViewModel { Projects = activeProjects });
                }
            }

            // Inactive non-expired subscriptions.
            else if (!activeSubscriptions.Any() && allSubscriptions.Any(s => s.EndAt <= DateTime.Now))
            {
                // Will suggest activating one of them (manually) and continue.
                if (!activeProjects.Any())
                {
                    // Project = null means the project selection won't be displayed.
                    return View("Inactive", new SelectProjectViewModel { Projects = null });
                }

                // Will suggest doing the same, alternatively allows to pick among active projects.
                else
                {
                    return View("Inactive", new SelectProjectViewModel { Projects = activeProjects });
                }
            }

            return null;
        }

        private ActionResult SetSharedProject(string token, string message)
        {
            return SetProject(token, Guid.Parse(SHARED_PROJECT_ID), DateTime.MaxValue, $"{message} The app will use the shared Project ID (\"{SHARED_PROJECT_ID}\").");
        }

        private async Task<ActionResult> DeploySampleInOwnedSubscriptionAsync(string token, UserModel user, IEnumerable<SubscriptionModel> subscriptions)
        {
            var administeredSubscriptionId = user.AdministeredSubscriptions.FirstOrDefault()?.SubscriptionId.Value;

            if (administeredSubscriptionId != null)
            {
                var project = await DeploySampleAsync(token, administeredSubscriptionId.Value);

                return SetProject(token, project.ProjectId.Value, subscriptions.FirstOrDefault(s => s.SubscriptionId == administeredSubscriptionId.Value).EndAt.Value);
            }
            else
            {
                return SetSharedProject(token, "There was an error when creating the sample site.");
            }
        }

        private ActionResult SetProject(string token, Guid projectId, DateTime subscriptionExpiresAt, string message = null)
        {
            if (projectId != Guid.Empty)
            {
                SetIdAndRename(token, projectId, subscriptionExpiresAt);

                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(message);
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult($"There was a problem when setting the \"ProjectId\" environment variable to \"{projectId}\". You can set it up manually in your environment (App service - Application Settings, web.config etc.).");
            }
        }

        private void SetIdAndRename(string token, Guid projectId, DateTime subscriptionExpiresAt)
        {
            AppSettingProvider.ProjectId = projectId;
            AppSettingProvider.SubscriptionExpiresAt = subscriptionExpiresAt;

            // Fire-and-forget. Assigned to 'task' just to avoid warnings.
            var task = RenameProjectAsync(token, projectId);
        }

        private async Task RenameProjectAsync(string token, Guid projectId)
        {
            string deployedAt = DateTime.Now.ToString("m");
            var project = new ProjectModel
            {
                ProjectId = projectId,
                ProjectName = string.Format(PROJECT_RENAME_PATTERN, deployedAt),
                ProjectType = "deliver"
            };

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{AdminHelpers.KC_BASE_URL}project/{projectId}"))
            {
                string contentString = JsonConvert.SerializeObject(project, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await AdminHelpers.GetStandardResponseAsync(token, _httpClient, request);
                response.Dispose();
            }
        }

        private async Task<(SubscriptionModel subscription, ProjectModel project)> StartTrialAndSampleAsync(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{AdminHelpers.KC_BASE_URL}subscription/trial"))
            {
                using (HttpResponseMessage response = await AdminHelpers.GetStandardResponseAsync(token, _httpClient, request))
                {
                    var subscription = await AdminHelpers.GetResultAsync<SubscriptionModel>(response);

                    if (subscription?.SubscriptionId != null)
                    {
                        var project = await DeploySampleAsync(token, subscription.SubscriptionId.Value);

                        return (subscription, project);
                    }
                    else
                    {
                        return (null, null);
                    }
                }
            }
        }

        private async Task<ProjectModel> DeploySampleAsync(string token, Guid subscriptionId)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{AdminHelpers.KC_BASE_URL}project/sample/undersubscription/{subscriptionId}"))
            {
                using (HttpResponseMessage response = await AdminHelpers.GetStandardResponseAsync(token, _httpClient, request))
                {
                    return await AdminHelpers.GetResultAsync<ProjectModel>(response);
                }
            }
        }

        private async Task<IEnumerable<ProjectModel>> GetProjectsAsync(string token, bool onlyActive = false)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{AdminHelpers.KC_BASE_URL}project"))
            {
                using (HttpResponseMessage response = await AdminHelpers.GetStandardResponseAsync(token, _httpClient, request))
                {
                    var projects = await AdminHelpers.GetResultAsync<IEnumerable<ProjectModel>>(response);

                    return onlyActive ? projects : projects.Where(p => p.Inactive == false);
                }
            }
        }

        private string GetToken()
        {
            return Request.Cookies[COOKIE_NAME]?.Value;
        }

        private async Task<IEnumerable<SubscriptionModel>> GetSubscriptionsAsync(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{AdminHelpers.KC_BASE_URL}subscription"))
            using (HttpResponseMessage response = await AdminHelpers.GetStandardResponseAsync(token, _httpClient, request))
            {
                return await AdminHelpers.GetResultAsync<IEnumerable<SubscriptionModel>>(response);
            }
        }

        private void AddCookie(string token)
        {
            if (string.IsNullOrEmpty(Request.Cookies["kcToken"]?.Value))
            {
                var cookie = new HttpCookie("kcToken", token) { Expires = DateTime.Now.AddDays(1) };
                Response.Cookies.Add(cookie);
            }
        }

        private static SelectProjectViewModel BuildSelectProjectViewModel(IEnumerable<ProjectModel> activeProjects)
        {
            return new SelectProjectViewModel
            {
                Projects = activeProjects
            };
        }
    }
}