using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using System.Net.Http;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Helpers
{
    public static class AdminHelpers
    {
        public const string KC_BASE_URL = @"https://app.kenticocloud.com/api/";
        //public const string KC_BASE_URL = @"https://kenticolabs-cdn-develop.azurewebsites.net/api/";

        public static async Task<HttpResponseMessage> GetStandardResponseAsync(string token, HttpClient httpClient, HttpRequestMessage request)
        {
            AddStandardHeaders(token, request);
            HttpResponseMessage response = await httpClient.SendAsync(request);

            return response;
        }

        public static void AddStandardHeaders(string token, HttpRequestMessage request)
        {
            request.Headers.Add("X-Auth", token);
        }

        public static async Task<Models.UserModel> GetUserAsync(string token, HttpClient httpClient, string baseUrl)
        {
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}auth"))
            using (HttpResponseMessage response = await GetStandardResponseAsync(token, httpClient, request))
            {
                return await GetResultAsync<Models.UserModel>(response);
            }
        }

        public static async Task<TResult> GetResultAsync<TResult>(HttpResponseMessage response)
            where TResult : class
        {
            TResult result = null;

            // TODO Deal with it upper in the call stack.
            try
            {
                result = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }
    }
}