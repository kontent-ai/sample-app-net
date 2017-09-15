using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using DancingGoat.Areas.Admin.Models;
using KenticoCloud.Delivery;
using Newtonsoft.Json;
using System.Threading;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class ProjectProvider : ProviderBase
    {
        public const string PROJECT_RENAME_PATTERN = "Sample Project (MVC Sample App, {0})";
        public const int PROJECT_EXISTENCE_VERIFICATION_RETRY_INTERVAL = 1;
        public const int PROJECT_EXISTENCE_VERIFICATION_RETRY_COUNT = 120;
        public const int PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS = 32;

        public ProjectProvider(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<IEnumerable<ProjectModel>> GetProjectsAsync(string token, bool onlyActive = false)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KenticoCloudApiUrl}project"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, request))
                {
                    var projects = await GetResultAsync<IEnumerable<ProjectModel>>(response);

                    return onlyActive ? projects : projects?.Where(p => p.Inactive == false);
                }
            }
        }

        public async Task<ProjectModel> DeploySampleAsync(string token, Guid subscriptionId)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KenticoCloudApiUrl}project/sample/undersubscription/{subscriptionId}"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, request))
                {
                    var project = await GetResultAsync<ProjectModel>(response);

                    if (project != null)
                    {
                        var client = new DeliveryClient(project.ProjectId.ToString());
                        IEnumerable<ContentItem> items;
                        int i = 0;

                        do
                        {
                            items = (await client.GetItemsAsync()).Items;
                            i++;
                            Thread.Sleep(PROJECT_EXISTENCE_VERIFICATION_RETRY_INTERVAL * 1000);
                        } while (i < PROJECT_EXISTENCE_VERIFICATION_RETRY_COUNT && (items == null || items.Count() < PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS));

                        if (items.Count() >= PROJECT_EXISTENCE_VERIFICATION_REQUIRED_ITEMS)
                        {
                            return project;
                        }
                    }

                    throw new DeliveryException(System.Net.HttpStatusCode.NotFound, "There was an error creating the project in Kentico Cloud. Check the project and set its project ID as a \"ProjectId\" environment variable.");
                }
            }
        }

        public async Task RenameProjectAsync(string token, Guid projectId)
        {
            string deployedAt = DateTime.Now.ToString("m");
            var project = new ProjectModel
            {
                ProjectId = projectId,
                ProjectName = string.Format(PROJECT_RENAME_PATTERN, deployedAt),
                ProjectType = "deliver"
            };

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{KenticoCloudApiUrl}project/{projectId}"))
            {
                string contentString = JsonConvert.SerializeObject(project, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await GetResponseAsync(token, request);
                response.Dispose();
            }
        }
    }
}