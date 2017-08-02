using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using DancingGoat.Areas.Admin.Models;
using System.Text;

namespace DancingGoat.Areas.Admin.Helpers
{
    public static class AdminHelpers
    {
        public const string KC_BASE_URL = @"https://app.kenticocloud.com/api/";
        public const string STATUS_ACTIVE = "active";
        public const string SHARED_PROJECT_ID = "975bf280-fd91-488c-994c-2f04416e5ee3";
        public const string PROJECT_RENAME_PATTERN = "Sample Project (MVC Sample App, {0})";

        public static async Task<HttpResponseMessage> GetResponseAsync(string token, HttpClient httpClient, HttpRequestMessage request)
        {
            AddStandardHeaders(token, request);
            return await httpClient.SendAsync(request);
        }

        public static void AddStandardHeaders(string token, HttpRequestMessage request)
        {
            request.Headers.Add("X-Auth", token);
        }

        public static async Task<UserModel> GetUserAsync(string token, HttpClient httpClient, string baseUrl)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}auth"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, httpClient, request))
                {
                    return await GetResultAsync<UserModel>(response);
                }
            }
        }

        public static async Task<TResult> GetResultAsync<TResult>(HttpResponseMessage response)
            where TResult : class
        {
            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<AdminHelperResults> CheckInactiveSubscriptions(string token, HttpClient httpClient, IEnumerable<SubscriptionModel> subscriptions = null, IEnumerable<ProjectModel> projects = null)
        {
            var activeProjects = projects?.Where(p => p.Inactive == false) ?? await GetProjectsAsync(token, httpClient);
            var allSubscriptions = subscriptions ?? await GetSubscriptionsAsync(token, httpClient);
            var activeSubscriptions = allSubscriptions?.Where(s => s.Status.Equals(STATUS_ACTIVE, StringComparison.InvariantCultureIgnoreCase));

            // Inactive expired subscriptions. 
            if (!activeSubscriptions.Any() && !allSubscriptions.All(s => s.EndAt > DateTime.Now))
            {
                var latestExpiredSubscriptions = allSubscriptions.OrderByDescending(s => s.EndAt);

                // Will allow to either convert to the free plan or to use the shared project.
                if (!activeProjects.Any())
                {
                    return new AdminHelperResults
                    {
                        EndAt = latestExpiredSubscriptions.FirstOrDefault().EndAt.Value,
                        ViewName = "Expired",

                        // Projects = null means the project selection won't be displayed.
                        Model = new SelectProjectViewModel { Projects = null }
                    };
                }

                // Will allow to either convert to the free plan, to use the shared project, or to pick among active projects.
                else
                {
                    return new AdminHelperResults
                    {
                        EndAt = latestExpiredSubscriptions.FirstOrDefault().EndAt.Value,
                        ViewName = "Expired",
                        Model = new SelectProjectViewModel { Projects = activeProjects }
                    };
                }
            }

            // Inactive non-expired subscriptions.
            else if (!activeSubscriptions.Any() && allSubscriptions.Any(s => s.EndAt <= DateTime.Now))
            {
                // Will suggest activating subscriptions (manually) and continue.
                if (!activeProjects.Any())
                {
                    return new AdminHelperResults
                    {
                        ViewName = "Inactive",
                        Model = new SelectProjectViewModel { Projects = null }
                    };
                }

                // Will suggest activating subscriptions (manually) or picking among active projects.
                else
                {
                    return new AdminHelperResults
                    {
                        ViewName = "Inactive",
                        Model = new SelectProjectViewModel { Projects = activeProjects }
                    };
                }
            }

            return null;
        }

        public static async Task<ActionResult> SetSharedProjectAsync(string token, HttpClient httpClient, string message)
        {
            return await SetProjectAsync(token, httpClient, Guid.Parse(SHARED_PROJECT_ID), DateTime.MaxValue, $"{message} The app will use the shared project ID (\"{SHARED_PROJECT_ID}\").");
        }

        public static async Task<ActionResult> DeploySampleInOwnedSubscriptionAsync(string token, HttpClient httpClient, UserModel user, IEnumerable<SubscriptionModel> subscriptions = null)
        {
            var administeredSubscriptionId = user.AdministeredSubscriptions.FirstOrDefault()?.SubscriptionId;
            var actualSubscriptions = subscriptions ?? await GetSubscriptionsAsync(token, httpClient);
            var administeredSubscription = actualSubscriptions.FirstOrDefault(s => s.SubscriptionId == administeredSubscriptionId);

            if (administeredSubscriptionId.HasValue && administeredSubscription != null)
            {
                var project = await DeploySampleAsync(token, httpClient, administeredSubscriptionId.Value);

                return await SetProjectAsync(token, httpClient, project.ProjectId.Value, administeredSubscription.CurrentPlan.EndAt.Value);
            }
            else
            {
                return await SetSharedProjectAsync(token, httpClient, "There was an error when creating the sample site.");
            }
        }

        public static async Task<ActionResult> SetProjectAsync(string token, HttpClient httpClient, Guid projectId, DateTime subscriptionExpiresAt, string message = null)
        {
            if (projectId != Guid.Empty)
            {
                await SetIdAndRenameAsync(token, httpClient, projectId, subscriptionExpiresAt);

                return DancingGoat.Helpers.RedirectHelpers.GetHomeRedirectResult(message);
            }
            else
            {
                return DancingGoat.Helpers.RedirectHelpers.GetSelfConfigRecheckResult($"There was a problem when setting the \"ProjectId\" environment variable to \"{projectId}\". You can set it up manually in your environment (App service - Application Settings, web.config etc.).");
            }
        }

        public static async Task SetIdAndRenameAsync(string token, HttpClient httpClient, Guid projectId, DateTime subscriptionExpiresAt)
        {
            AppSettingProvider.ProjectId = projectId;
            AppSettingProvider.SubscriptionExpiresAt = subscriptionExpiresAt;

            await RenameProjectAsync(token, httpClient, projectId);
        }

        public static async Task RenameProjectAsync(string token, HttpClient httpClient, Guid projectId)
        {
            string deployedAt = DateTime.Now.ToString("m");
            var project = new ProjectModel
            {
                ProjectId = projectId,
                ProjectName = string.Format(PROJECT_RENAME_PATTERN, deployedAt),
                ProjectType = "deliver"
            };

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{KC_BASE_URL}project/{projectId}"))
            {
                string contentString = JsonConvert.SerializeObject(project, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await GetResponseAsync(token, httpClient, request);
                response.Dispose();
            }
        }

        public static async Task<(SubscriptionModel subscription, ProjectModel project)> StartTrialAndSampleAsync(string token, HttpClient httpClient)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}subscription/trial"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, httpClient, request))
                {
                    var subscription = await GetResultAsync<SubscriptionModel>(response);

                    if (subscription != null)
                    {
                        var project = await DeploySampleAsync(token, httpClient, subscription.SubscriptionId);

                        return (subscription, project);
                    }
                    else
                    {
                        return (null, null);
                    }
                }
            }
        }

        public static async Task<ProjectModel> DeploySampleAsync(string token, HttpClient httpClient, Guid subscriptionId)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}project/sample/undersubscription/{subscriptionId}"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, httpClient, request))
                {
                    return await GetResultAsync<ProjectModel>(response);
                }
            }
        }

        public static async Task<IEnumerable<ProjectModel>> GetProjectsAsync(string token, HttpClient httpClient, bool onlyActive = false)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}project"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, httpClient, request))
                {
                    var projects = await GetResultAsync<IEnumerable<ProjectModel>>(response);

                    return onlyActive ? projects : projects?.Where(p => p.Inactive == false);
                }
            }
        }

        public static async Task<IEnumerable<SubscriptionModel>> GetSubscriptionsAsync(string token, HttpClient httpClient)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}user/subscriptions"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, httpClient, request))
                {
                    return await GetResultAsync<IEnumerable<SubscriptionModel>>(response);
                }
            }
        }
    }
}