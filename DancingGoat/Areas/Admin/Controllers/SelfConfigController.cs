using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using DancingGoat.Helpers;
using DancingGoat.Areas.Admin.Infrastructure;
using DancingGoat.Areas.Admin.Helpers;
using DancingGoat.Areas.Admin.Models;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        protected const string MESSAGE_UNAUTHENTICATED = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.";
        protected const string CAPTION_CONFIGURATION_WRITE_ERROR = "Configuration Save Error";
        protected const string CAPTION_DESERIALIZATION_ERROR = "API Response Deserialization Error";
        protected const string MESSAGE_NEW_SAMPLE_PROJECT = "You've configured your app with a project ID \"{0}\" of the newly created sample project.";
        protected const string MESSAGE_SHARED_PROJECT = "You've configured your app to with a project ID of a shared Kentico Cloud project.";
        protected const string AUTHENTICATION_COOKIE_NAME = "kcToken";

        protected readonly HttpClient _httpClient = new HttpClient();
        protected readonly UserProvider _userProvider;
        protected readonly ProjectProvider _projectProvider;
        protected readonly SubscriptionProvider _subscriptionProvider;
        protected readonly SelfConfigManager _selfConfigManager;

        public SelfConfigController()
        {
            _userProvider = new UserProvider(_httpClient);
            _projectProvider = new ProjectProvider(_httpClient);
            _subscriptionProvider = new SubscriptionProvider(_httpClient, _projectProvider);
            _selfConfigManager = new SelfConfigManager(_subscriptionProvider, _projectProvider);
        }

        [HttpGet]
        public ActionResult Index()
        {
            AddSecurityInfoToViewBag();

            return View(new IndexViewModel { SignInModel = new SignInViewModel(), SelectProjectModel = new SelectProjectViewModel(), SignUpModel = new SignUpViewModel() });
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

                    var user = await _userProvider.GetUserAsync(actualToken);

                    IEnumerable<SubscriptionModel> subscriptions;

                    if (user != null)
                    {
                        subscriptions = await _subscriptionProvider.GetSubscriptionsAsync(actualToken);
                    }
                    else
                    {
                        return RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
                    }

                    if (subscriptions == null || !subscriptions.Any())
                    {
                        var subscriptionAndProject = await _subscriptionProvider.StartTrialAndSampleAsync(actualToken);

                        if (subscriptionAndProject.subscription != null && subscriptionAndProject.project != null)
                        {
                            try
                            {
                                _selfConfigManager.SetProjectIdAndExpirationAsync(subscriptionAndProject.project.ProjectId.Value, subscriptionAndProject.subscription.EndAt.Value);
                                await _projectProvider.RenameProjectAsync(token, subscriptionAndProject.project.ProjectId.Value);

                                return RedirectHelpers.GetHomeRedirectResult(string.Format(MESSAGE_NEW_SAMPLE_PROJECT, subscriptionAndProject.project.ProjectId.Value));
                            }
                            catch (ConfigurationErrorsException ex)
                            {
                                return RedirectHelpers.GetSelfConfigIndexResult(ex.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                _selfConfigManager.SetSharedProjectIdAsync();

                                return RedirectHelpers.GetHomeRedirectResult(MESSAGE_SHARED_PROJECT);
                            }
                            catch (ConfigurationErrorsException ex)
                            {
                                return RedirectHelpers.GetSelfConfigIndexResult(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        SubscriptionStatusResults results = await _subscriptionProvider.GetSubscriptionsStatusAsync(actualToken, subscriptions);

                        if (results.Status == SubscriptionStatus.Active)
                        {
                            if (results.Projects.Any())
                            {
                                AddSecurityInfoToViewBag();

                                return View("SelectOrCreateProject", new SelectProjectViewModel { Projects = results.Projects });
                            }
                            else
                            {
                                try
                                {
                                    var projectId = await _selfConfigManager.DeployAndSetSampleProject(actualToken, user);

                                    return RedirectHelpers.GetHomeRedirectResult(string.Format(MESSAGE_NEW_SAMPLE_PROJECT, projectId));
                                }
                                catch (ConfigurationErrorsException ex)
                                {
                                    return RedirectHelpers.GetSelfConfigIndexResult(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            ViewBag.EndAt = results.EndAt;

                            return View(results.Status.ToString(), new SelectProjectViewModel { Projects = results.Projects });
                        }
                    }
                }
                catch (JsonException ex)
                {
                    return RedirectHelpers.GetSelfConfigIndexResult($"{CAPTION_DESERIALIZATION_ERROR}: {ex.Message}");
                }
            }
            else
            {
                return RedirectHelpers.GetSelfConfigIndexResult(MESSAGE_UNAUTHENTICATED);
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
                        subscriptions = await _subscriptionProvider.GetSubscriptionsAsync(actualToken);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_UNAUTHENTICATED);
                    }

                    if (subscriptions == null || !subscriptions.Any())
                    {
                        return View("NoSubscriptions");
                    }
                    else
                    {
                        SubscriptionStatusResults results = await _subscriptionProvider.GetSubscriptionsStatusAsync(actualToken, subscriptions);

                        if (results.Status == SubscriptionStatus.Active)
                        {
                            if (results.Projects.Any(p => p.ProjectId == AppSettingProvider.ProjectId))
                            {
                                try
                                {
                                    AppSettingProvider.SubscriptionExpiresAt = subscriptions.FirstOrDefault(s => s.Projects.Any(p => p.Id == AppSettingProvider.ProjectId))?.CurrentPlan?.EndAt;
                                }
                                catch (ConfigurationErrorsException ex)
                                {
                                    return RedirectHelpers.GetSelfConfigRecheckResult(ex.Message);
                                }

                                return RedirectHelpers.GetHomeRedirectResult(null);
                            }
                            else
                            {
                                AddSecurityInfoToViewBag();

                                return View("SelectOrCreateProject", new SelectProjectViewModel { Projects = results.Projects });
                            }
                        }
                        else
                        {
                            ViewBag.EndAt = results.EndAt;

                            return View(results.Status.ToString(), new SelectProjectViewModel { Projects = results.Projects });
                        }
                    }
                }
                catch (JsonException ex)
                {
                    return RedirectHelpers.GetSelfConfigRecheckResult($"{CAPTION_DESERIALIZATION_ERROR}: {ex.Message}");
                }
            }
            else
            {
                return RedirectHelpers.GetSelfConfigRecheckResult(MESSAGE_UNAUTHENTICATED);
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> Free()
        {
            string token = GetToken();

            try
            {
                var subscription = await _subscriptionProvider.ConvertToFreeAsync(token);

                if (subscription != null)
                {
                    return RedirectHelpers.GetHomeRedirectResult($"You've converted your subscription {subscription.SubscriptionId} to a free plan.");
                }
                else
                {
                    return View("Error", new ErrorViewModel { Caption = "Too Many Resources", Message = "We couldn't convert your subscription to the free plan. Please make sure you've lowered your resources in Kentico Cloud to meet the limits of the free plan." });
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                return RedirectHelpers.GetHomeRedirectResult(ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SelectProject(SelectProjectViewModel model)
        {
            try
            {
                if (model.ProjectId != Guid.Empty)
                {
                    DateTime? endAt = null;

                    if (model.ManualInput.HasValue && model.ManualInput.Value)
                    {
                        string token = GetToken();

                        if ((await _userProvider.GetUserAsync(token)) != null)
                        {
                            var subscriptions = await _subscriptionProvider.GetSubscriptionsAsync(token);

                            // Projects may not always be owned by the user. Getting subscription EndAt date might not be possible. Hence the allowed null outcome.
                            endAt = subscriptions.FirstOrDefault(s => s.Projects.Any(p => p.Id == AppSettingProvider.ProjectId))?.CurrentPlan?.EndAt;
                        }
                        else
                        {
                            return View("Error", new ErrorViewModel { Caption = "Unauthenticated", Message = MESSAGE_UNAUTHENTICATED });
                        }
                    }

                    try
                    {
                        _selfConfigManager.SetProjectIdAndExpirationAsync(model.ProjectId, endAt);
                    }
                    catch (ConfigurationErrorsException ex)
                    {
                        return View("Error", new ErrorViewModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message });
                    }

                    return RedirectHelpers.GetHomeRedirectResult($"You've configured your app with a project ID \"{model.ProjectId}\".");
                }
                else
                {
                    try
                    {
                        _selfConfigManager.SetSharedProjectIdAsync();

                        return View("Error", new ErrorViewModel { Caption = "Missing project ID", Message = "The submitted project ID was an empty GUID. The app was configured with the shared project ID instead. You may wish to reconfigure the project ID in your environment settings (Azure application settings, web.config, etc.)." });
                    }
                    catch (ConfigurationErrorsException ex)
                    {
                        return View("Error", new ErrorViewModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message });
                    }
                }
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }

        [HttpPost]
        public ActionResult UseShared()
        {
            try
            {
                _selfConfigManager.SetProjectIdAndExpirationAsync(AppSettingProvider.DefaultProjectId.Value);

                return RedirectHelpers.GetHomeRedirectResult(MESSAGE_SHARED_PROJECT);
            }
            catch (ConfigurationErrorsException ex)
            {
                return View("Error", new ErrorViewModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message });
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> DeploySample()
        {
            string token = GetToken();

            try
            {
                var user = await _userProvider.GetUserAsync(token);

                try
                {
                    var projectId = await _selfConfigManager.DeployAndSetSampleProject(token, user);

                    return RedirectHelpers.GetHomeRedirectResult(string.Format(MESSAGE_NEW_SAMPLE_PROJECT, projectId));
                }
                catch (ConfigurationErrorsException ex)
                {
                    var activeProjects = await _projectProvider.GetProjectsAsync(token, true);
                    ViewBag.WarningMessage = ex.Message;

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
            return Request.Cookies[AUTHENTICATION_COOKIE_NAME]?.Value;
        }

        private void AddAuthenticationCookie(string token)
        {
            var cookie = new HttpCookie(AUTHENTICATION_COOKIE_NAME, token) { Expires = DateTime.Now.AddDays(1), HttpOnly = true };

            if (string.IsNullOrEmpty(Request.Cookies[AUTHENTICATION_COOKIE_NAME]?.Value))
            {
                Response.Cookies.Add(cookie);
            }
            else
            {
                Response.Cookies[AUTHENTICATION_COOKIE_NAME].Value = cookie.Value;
                Response.Cookies[AUTHENTICATION_COOKIE_NAME].Expires = cookie.Expires;
            }
        }

        private ActionResult GetDeserializationErrorResult(Exception ex)
        {
            return View("Error", new ErrorViewModel { Caption = CAPTION_DESERIALIZATION_ERROR, Message = ex.Message });
        }

        private void AddSecurityInfoToViewBag()
        {
            ViewBag.IsTls = Request.IsSecureConnection;
            ViewBag.IsLocal = Request.IsLocal;
        }
    }
}