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
using System.Configuration;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        protected const string MESSAGE_UNAUTHENTICATED = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.";
        protected const string MESSAGE_GENERAL_ERROR = "An unknown error occurred.";
        protected const string MESSAGE_CONFIGURATION_WRITE_ERROR_CAPTION = "Configuration Save Error";
        protected const string MESSAGE_CONFIGURATION_WRITE_ERROR = "There was an error when setting the project ID. Make sure the worker process has permissions to set the environment settings and try again.";
        protected const string MESSAGE_DESERIALIZATION_ERROR_CAPTION = "API Response Deserialization Error";
        protected const string COOKIE_NAME = "kcToken";

        protected readonly HttpClient _httpClient = new HttpClient();

        [HttpGet]
        public ActionResult Index()
        {
            AddSecurityInfoToViewBag();

            return View(new IndexViewModel() { SignInModel = new SignInViewModel(), SelectProjectModel = new SelectProjectViewModel(), SignUpModel = new SignUpViewModel() });
        }

        [HttpPost]
        public async Task<ActionResult> Index(string token)
        {
            string actualToken = token ?? GetToken();

            if (!string.IsNullOrEmpty(actualToken))
            {
                try
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        AddAuthenticationCookie(token);
                    }

                    var user = await AdminHelpers.GetUserAsync(actualToken, _httpClient, AdminHelpers.KC_BASE_URL);

                    IEnumerable<SubscriptionModel> subscriptions;

                    if (user != null)
                    {
                        subscriptions = await AdminHelpers.GetSubscriptionsAsync(actualToken, _httpClient);
                    }
                    else
                    {
                        return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
                    }

                    if (subscriptions == null || !subscriptions.Any())
                    {
                        var subscriptionAndProject = await AdminHelpers.StartTrialAndSampleAsync(actualToken, _httpClient);

                        if (subscriptionAndProject.subscription != null && subscriptionAndProject.project != null)
                        {
                            try
                            {
                                return await AdminHelpers.SetProjectAsync(actualToken, _httpClient, subscriptionAndProject.project.ProjectId.Value, subscriptionAndProject.subscription.EndAt.Value);
                            }
                            catch (ConfigurationErrorsException)
                            {
                                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_CONFIGURATION_WRITE_ERROR);
                            }
                        }
                        else
                        {
                            try
                            {
                                return await AdminHelpers.SetSharedProjectAsync(actualToken, _httpClient, "There was an error when creating the sample site.");
                            }
                            catch (ConfigurationErrorsException)
                            {
                                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_CONFIGURATION_WRITE_ERROR);
                            }
                        }
                    }
                    else
                    {
                        var activeProjects = await AdminHelpers.GetProjectsAsync(actualToken, _httpClient);
                        AdminHelperResults result = await AdminHelpers.CheckInactiveSubscriptions(actualToken, _httpClient, subscriptions, activeProjects);

                        if (string.IsNullOrEmpty(result?.ViewName))
                        {
                            if (activeProjects.Any())
                            {
                                AddSecurityInfoToViewBag();

                                return View("SelectOrCreateProject", new SelectProjectViewModel { Projects = activeProjects });
                            }
                            else
                            {
                                try
                                {
                                    return await AdminHelpers.DeploySampleInOwnedSubscriptionAsync(actualToken, _httpClient, user);
                                }
                                catch (ConfigurationErrorsException)
                                {
                                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_CONFIGURATION_WRITE_ERROR);
                                }
                            }
                        }

                        ViewBag.EndAt = result.EndAt;

                        return View(result.ViewName, result.Model);
                    }
                }
                catch (JsonException ex)
                {
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigIndexResult($"{MESSAGE_DESERIALIZATION_ERROR_CAPTION}: {ex.Message}");
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
            AddSecurityInfoToViewBag();

            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Recheck(string token)
        {
            string actualToken = token ?? GetToken();

            if (!string.IsNullOrEmpty(actualToken))
            {
                try
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        AddAuthenticationCookie(token);
                    }

                    IEnumerable<SubscriptionModel> subscriptions;

                    try
                    {
                        subscriptions = await AdminHelpers.GetSubscriptionsAsync(actualToken, _httpClient);
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
                        var activeProjects = await AdminHelpers.GetProjectsAsync(actualToken, _httpClient);

                        // The trial period has expired but the web job hasn't converted the trial to the free plan yet.
                        AdminHelperResults result = await AdminHelpers.CheckInactiveSubscriptions(actualToken, _httpClient, subscriptions, activeProjects);

                        // The trial was converted to the free plan by the web job.
                        if (string.IsNullOrEmpty(result?.ViewName))
                        {
                            if (activeProjects.Any(p => p.ProjectId == AppSettingProvider.ProjectId))
                            {
                                var user = await AdminHelpers.GetUserAsync(actualToken, _httpClient, AdminHelpers.KC_BASE_URL);

                                try
                                {
                                    AppSettingProvider.SubscriptionExpiresAt = subscriptions.FirstOrDefault(s => s.Projects.Any(p => p.Id == AppSettingProvider.ProjectId))?.CurrentPlan?.EndAt;
                                }
                                catch (ConfigurationErrorsException)
                                {
                                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_CONFIGURATION_WRITE_ERROR);
                                }

                                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(null);
                            }
                            else
                            {
                                AddSecurityInfoToViewBag();

                                return View("SelectOrCreateProject", new SelectProjectViewModel { Projects = activeProjects });
                            }
                        }

                        ViewBag.EndAt = result.EndAt;

                        return View(result.ViewName, result.Model);
                    }
                }
                catch (JsonException ex)
                {
                    return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult($"{MESSAGE_DESERIALIZATION_ERROR_CAPTION}: {ex.Message}");
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
            SubscriptionModel subscription;

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{AdminHelpers.KC_BASE_URL}subscription/free"))
            {
                using (HttpResponseMessage response = await AdminHelpers.GetResponseAsync(token, _httpClient, request))
                {
                    try
                    {
                        subscription = await AdminHelpers.GetResultAsync<SubscriptionModel>(response);

                        if (subscription != null && subscription.PlanName == "free")
                        {
                            try
                            {
                                AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;
                            }
                            catch (ConfigurationErrorsException)
                            {
                                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(MESSAGE_CONFIGURATION_WRITE_ERROR);
                            }

                            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've converted your subscription to a free plan.");
                        }
                        else
                        {
                            return View("Error", new ErrorViewModel { Caption = "Too Many Resources", Message = "We couldn't convert your subscription to the free plan. Please make sure you've lowered your resources in Kentico Cloud to meet the limits of the free plan." });
                        }
                    }
                    catch (JsonSerializationException ex)
                    {
                        return GetDeserializationErrorResult(ex);
                    }
                }
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> SelectProject(SelectProjectViewModel model)
        {
            string token = GetToken();

            try
            {
                if (model.ProjectId != Guid.Empty)
                {
                    var user = await AdminHelpers.GetUserAsync(token, _httpClient, AdminHelpers.KC_BASE_URL);
                    var subscriptions = await AdminHelpers.GetSubscriptionsAsync(token, _httpClient);

                    try
                    {
                        AppSettingProvider.ProjectId = model.ProjectId;
                        AppSettingProvider.SubscriptionExpiresAt = subscriptions.FirstOrDefault(s => s.Projects.Any(p => p.Id == AppSettingProvider.ProjectId))?.CurrentPlan?.EndAt;
                    }
                    catch (ConfigurationErrorsException)
                    {
                        return View("Error", new ErrorViewModel { Caption = MESSAGE_CONFIGURATION_WRITE_ERROR_CAPTION, Message = MESSAGE_CONFIGURATION_WRITE_ERROR });
                    }

                    return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult($"You've configured your app with a project ID \"{model.ProjectId}\".");
                }
                else
                {
                    try
                    {
                        return await AdminHelpers.SetSharedProjectAsync(token, _httpClient, "The project ID could not be found.");
                    }
                    catch (ConfigurationErrorsException)
                    {
                        return View("Error", new ErrorViewModel { Caption = MESSAGE_CONFIGURATION_WRITE_ERROR_CAPTION, Message = MESSAGE_CONFIGURATION_WRITE_ERROR });
                    }
                }
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public ActionResult UseShared()
        {
            try
            {
                AppSettingProvider.ProjectId = AppSettingProvider.DefaultProjectId;
                AppSettingProvider.SubscriptionExpiresAt = DateTime.MaxValue;
            }
            catch (ConfigurationErrorsException)
            {
                return View("Error", new ErrorViewModel { Caption = MESSAGE_CONFIGURATION_WRITE_ERROR_CAPTION, Message = MESSAGE_CONFIGURATION_WRITE_ERROR });
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }

            return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult("You've configured your app to with a project ID of a shared Kentico Cloud project.");
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> DeploySample()
        {
            string token = GetToken();
            UserModel user;

            try
            {
                user = await AdminHelpers.GetUserAsync(token, _httpClient, AdminHelpers.KC_BASE_URL);

                try
                {
                    return await AdminHelpers.DeploySampleInOwnedSubscriptionAsync(token, _httpClient, user);
                }
                catch (ConfigurationErrorsException)
                {
                    var activeProjects = await AdminHelpers.GetProjectsAsync(token, _httpClient, true);
                    ViewBag.WarningMessage = MESSAGE_CONFIGURATION_WRITE_ERROR;

                    return View("SelectOrCreateProjects", new SelectProjectViewModel { Projects = activeProjects });
                }
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }

        private string GetToken()
        {
            return Request.Cookies[COOKIE_NAME]?.Value;
        }

        private void AddAuthenticationCookie(string token)
        {
            var cookie = new HttpCookie(COOKIE_NAME, token);
            cookie.Expires = DateTime.Now.AddDays(1);

            if (string.IsNullOrEmpty(Request.Cookies[COOKIE_NAME]?.Value))
            {
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies[COOKIE_NAME].Value = cookie.Value;
                Response.Cookies[COOKIE_NAME].Expires = cookie.Expires;
            }
        }

        private ActionResult GetDeserializationErrorResult(Exception ex)
        {
            return View("Error", new ErrorViewModel { Caption = MESSAGE_DESERIALIZATION_ERROR_CAPTION, Message = ex.Message });
        }

        private void AddSecurityInfoToViewBag()
        {
            ViewBag.IsTls = Request.IsSecureConnection;
            ViewBag.IsLocal = Request.IsLocal;
        }
    }
}