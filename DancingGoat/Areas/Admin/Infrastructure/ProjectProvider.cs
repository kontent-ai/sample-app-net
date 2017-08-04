using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Models;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class ProjectProvider : ProviderBase
    {
        public const string PROJECT_RENAME_PATTERN = "Sample Project (MVC Sample App, {0})";

        public ProjectProvider(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<IEnumerable<ProjectModel>> GetProjectsAsync(string token, bool onlyActive = false)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}project"))
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
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{KC_BASE_URL}project/sample/undersubscription/{subscriptionId}"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, request))
                {
                    return await GetResultAsync<ProjectModel>(response);
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

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{KC_BASE_URL}project/{projectId}"))
            {
                string contentString = JsonConvert.SerializeObject(project, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.Content = new StringContent(contentString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await GetResponseAsync(token, request);
                response.Dispose();
            }
        }
    }
}