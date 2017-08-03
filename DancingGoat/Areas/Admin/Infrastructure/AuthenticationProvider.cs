using System.Net.Http;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Models;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class AuthenticationProvider : ProviderBase
    {
        public AuthenticationProvider(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<UserModel> GetUserAsync(string token)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{KC_BASE_URL}auth"))
            {
                using (HttpResponseMessage response = await GetResponseAsync(token, request))
                {
                    return await GetResultAsync<UserModel>(response);
                }
            }
        }
    }
}