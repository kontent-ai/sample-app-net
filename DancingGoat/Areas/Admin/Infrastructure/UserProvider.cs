using System.Net.Http;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Models;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class UserProvider : ProviderBase
    {
        public UserProvider(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<UserModel> GetUserAsync(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KenticoCloudApiUrl}auth"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return await GetResultAsync<UserModel>(response);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}