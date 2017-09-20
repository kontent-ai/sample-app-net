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
using KenticoCloud.Delivery;

namespace DancingGoat.Areas.Admin.Controllers
{
    public class SelfConfigController : Controller
    {
        protected const string MESSAGE_UNAUTHENTICATED = "You haven't authenticated with proper Kentico Cloud credentials. Please close the browser window and log in.";
        protected const string CAPTION_CONFIGURATION_WRITE_ERROR = "Configuration Save Error";
        protected const string CAPTION_DESERIALIZATION_ERROR = "API Response Deserialization Error";
        protected const string MESSAGE_NEW_SAMPLE_PROJECT = "You've configured your app with a project ID \"{0}\" of the newly created sample project. You can edit its contents at https://app.kenticocloud.com/.";
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
                        return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = MESSAGE_UNAUTHENTICATED, MessageType = MessageType.Error });
                    }

                    if (subscriptions == null || !subscriptions.Any())
                    {
                        SubscriptionModel subscription = await _subscriptionProvider.StartTrial(actualToken);
                        ProjectModel project = null;

                        try
                        {
                            if (subscription != null)
                            {
                                project = await _projectProvider.DeploySampleAsync(token, subscription.SubscriptionId);
                            }
                        }
                        catch (DeliveryException)
                        {
                            return UseSharedPrivate();
                        }

                        if (subscription != null && project != null)
                        {
                            try
                            {
                                _selfConfigManager.SetProjectIdAndExpirationAsync(project.ProjectId.Value, subscription.EndAt);
                                await _projectProvider.RenameProjectAsync(token, project.ProjectId.Value);

                                return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = string.Format(MESSAGE_NEW_SAMPLE_PROJECT, project.ProjectId.Value), MessageType = MessageType.Info });
                            }
                            catch (ConfigurationErrorsException ex)
                            {
                                return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error });
                            }
                        }
                        else
                        {
                            try
                            {
                                _selfConfigManager.SetSharedProjectIdAsync();

                                return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = MESSAGE_SHARED_PROJECT, MessageType = MessageType.Info });
                            }
                            catch (ConfigurationErrorsException ex)
                            {
                                return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error });
                            }
                        }
                    }
                    else
                    {
                        SubscriptionStatusResults results = await _subscriptionProvider.GetSubscriptionsStatusAsync(actualToken, subscriptions);

                        if (results.Status == SubscriptionStatus.Active)
                        {
                            if (results.Projects.Any(p => p.Inactive == false))
                            {
                                AddSecurityInfoToViewBag();

                                return View("SelectOrCreateProject", new SelectProjectViewModel { Projects = results.Projects.Where(p => p.Inactive == false) });
                            }
                            else
                            {
                                try
                                {
                                    var projectId = await _selfConfigManager.DeployAndSetSampleProject(actualToken, user);

                                    return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = string.Format(MESSAGE_NEW_SAMPLE_PROJECT, projectId), MessageType = MessageType.Info });
                                }
                                catch (ConfigurationErrorsException ex)
                                {
                                    return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error });
                                }
                                catch (DeliveryException ex)
                                {
                                    return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.EndAt = results.EndAt;

                            return View(results.Status.ToString(), new SelectProjectViewModel { Projects = results.Projects.Where(p => p.Inactive == false) });
                        }
                    }
                }
                catch (JsonException ex)
                {
                    return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = $"{CAPTION_DESERIALIZATION_ERROR}: {ex.Message}", MessageType = MessageType.Error });
                }
            }
            else
            {
                return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = MESSAGE_UNAUTHENTICATED, MessageType = MessageType.Error });
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
                        return RedirectHelpers.GetSelfConfigRecheckResult(new MessageModel { Caption = null, Message = MESSAGE_UNAUTHENTICATED, MessageType = MessageType.Error });
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
                            if (results.Projects.Any(p => p.ProjectId == AppSettingProvider.ProjectId && p.Inactive == false))
                            {
                                try
                                {
                                    AppSettingProvider.SubscriptionExpiresAt = subscriptions.FirstOrDefault(s => s.Projects.Any(p => p.Id == AppSettingProvider.ProjectId))?.CurrentPlan?.EndAt;
                                }
                                catch (ConfigurationErrorsException ex)
                                {
                                    return RedirectHelpers.GetSelfConfigRecheckResult(new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error });
                                }

                                return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = "The previously known expiration date of your subscription does not apply anymore. You can run your sample app as before.", MessageType = MessageType.Info });
                            }
                            else
                            {
                                AddSecurityInfoToViewBag();

                                return View("SelectOrCreateProject", new SelectProjectViewModel { Projects = results.Projects.Where(p => p.Inactive == false) });
                            }
                        }
                        else
                        {
                            ViewBag.EndAt = results.EndAt;

                            return View(results.Status.ToString(), new SelectProjectViewModel { Projects = results.Projects.Where(p => p.Inactive == false) });
                        }
                    }
                }
                catch (JsonException ex)
                {
                    return RedirectHelpers.GetSelfConfigRecheckResult(new MessageModel { Caption = null, Message = $"{CAPTION_DESERIALIZATION_ERROR}: {ex.Message}", MessageType = MessageType.Error });
                }
            }
            else
            {
                return RedirectHelpers.GetSelfConfigRecheckResult(new MessageModel { Caption = null, Message = MESSAGE_UNAUTHENTICATED, MessageType = MessageType.Error });
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
                    return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = $"You've converted your subscription {subscription.SubscriptionId} to a free plan.", MessageType = MessageType.Info });
                }
                else
                {
                    return View("Error", new MessageModel { Caption = "Too Many Resources", Message = "We couldn't convert your subscription to the free plan. Please make sure you've lowered your resources in Kentico Cloud to meet the limits of the free plan.", MessageType = MessageType.Error });
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error });
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
                if (model.ProjectId.HasValue)
                {
                    DateTime? endAt = null;

                    if (model.ManualInput.HasValue && model.ManualInput.Value)
                    {
                        string token = GetToken();

                        if ((await _userProvider.GetUserAsync(token)) != null)
                        {
                            var subscriptions = await _subscriptionProvider.GetSubscriptionsAsync(token);

                            // The user does not have to be logged in and projects may not always be owned by the user. Getting subscription EndAt date might not be possible. Hence the allowed null outcome.
                            endAt = subscriptions.FirstOrDefault(s => s.Projects.Any(p => p.Id == AppSettingProvider.ProjectId))?.CurrentPlan?.EndAt;
                        }
                    }

                    try
                    {
                        _selfConfigManager.SetProjectIdAndExpirationAsync(model.ProjectId.Value, endAt);
                    }
                    catch (ConfigurationErrorsException ex)
                    {
                        return View("Error", new MessageModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message, MessageType = MessageType.Error });
                    }

                    return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = $"You've configured your app with a project ID \"{model.ProjectId}\".", MessageType = MessageType.Info });
                }
                else
                {
                    try
                    {
                        _selfConfigManager.SetSharedProjectIdAsync();

                        return View("Error", new MessageModel { Caption = "Missing project ID", Message = "The submitted project ID was an empty GUID. The app was configured with the shared project ID instead. You may wish to reconfigure the project ID in your environment settings (Azure application settings, web.config, etc.).", MessageType = MessageType.Error });
                    }
                    catch (ConfigurationErrorsException ex)
                    {
                        return View("Error", new MessageModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message, MessageType = MessageType.Error });
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
            return UseSharedPrivate();
        }

        [HttpPost]
        [KenticoCloudAuthorize]
        public async Task<ActionResult> DeploySample()
        {
            string token = GetToken();

            try
            {
                var user = await _userProvider.GetUserAsync(token);

                if (user != null)
                {
                    try
                    {
                        var projectId = await _selfConfigManager.DeployAndSetSampleProject(token, user);

                        return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = string.Format(MESSAGE_NEW_SAMPLE_PROJECT, projectId), MessageType = MessageType.Info });
                    }
                    catch (ConfigurationErrorsException ex)
                    {
                        return await HandleSampleDeploymentExceptions(token, ex);
                    }
                    catch (DeliveryException ex)
                    {
                        return await HandleSampleDeploymentExceptions(token, ex);
                    }
                }
                else
                {
                    return RedirectHelpers.GetSelfConfigIndexResult(new MessageModel { Caption = null, Message = MESSAGE_UNAUTHENTICATED, MessageType = MessageType.Error });
                }
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }

        private async Task<ActionResult> HandleSampleDeploymentExceptions(string token, Exception ex)
        {
            var activeProjects = await _projectProvider.GetProjectsAsync(token, true);
            ViewBag.Message = new MessageModel { Caption = null, Message = ex.Message, MessageType = MessageType.Error };

            return View("SelectOrCreateProjects", new SelectProjectViewModel { Projects = activeProjects });
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
            return View("Error", new MessageModel { Caption = CAPTION_DESERIALIZATION_ERROR, Message = ex.Message, MessageType = MessageType.Error });
        }

        private void AddSecurityInfoToViewBag()
        {
            ViewBag.IsTls = Request.IsSecureConnection;
            ViewBag.IsLocal = Request.IsLocal;
        }

        private ActionResult UseSharedPrivate()
        {
            try
            {
                _selfConfigManager.SetProjectIdAndExpirationAsync(AppSettingProvider.DefaultProjectId.Value);

                return RedirectHelpers.GetHomeRedirectResult(new MessageModel { Caption = null, Message = MESSAGE_SHARED_PROJECT, MessageType = MessageType.Info });
            }
            catch (ConfigurationErrorsException ex)
            {
                return View("Error", new MessageModel { Caption = CAPTION_CONFIGURATION_WRITE_ERROR, Message = ex.Message, MessageType = MessageType.Error });
            }
            catch (JsonSerializationException ex)
            {
                return GetDeserializationErrorResult(ex);
            }
        }
    }
}