using System;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public abstract class ProviderBase
    {
        public const string STATUS_ACTIVE = "active";

        protected readonly HttpClient _httpClient;

        public string KenticoCloudApiUrl
        {
            get
            {
                return AppSettingProvider.KenticoCloudUrl + "api/";
            }
        }

        protected ProviderBase(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        protected async Task<HttpResponseMessage> GetResponseAsync(string token, HttpRequestMessage request)
        {
            request.Headers.Add("X-Auth", token);
            return await _httpClient.SendAsync(request);
        }

        protected async Task<TResult> GetResultAsync<TResult>(HttpResponseMessage response)
            where TResult : class
        {
            return JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
        }
    }
}