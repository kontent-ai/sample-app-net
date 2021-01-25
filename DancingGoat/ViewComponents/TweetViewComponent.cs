using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;

namespace DancingGoat.ViewComponents
{
    public class TweetViewComponent : ViewComponent
    {
        private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(30) };

        public TweetViewComponent()
        {
        }

        public IViewComponentResult Invoke(Tweet item)
        {
            var selectedTheme = item.Theme.FirstOrDefault()?.Name?.ToLower() ?? "light";
            var displayOptions = item.DisplayOptions.ToList();
            var hideThread = displayOptions.Any(o => o.Codename.Equals("hide_thread"));
            var hideMedia = displayOptions.Any(o => o.Codename.Equals("hide_media"));
            var options = $"&hide_thread={hideThread}&hide_media={hideMedia}";
            var tweetLink = item.TweetLink;

            var tweetResponse = Client.GetAsync(
                $"https://publish.twitter.com/oembed?url={tweetLink}&theme={selectedTheme}" + options).Result;
            var jsonResponse = JObject.Parse(tweetResponse.Content.ReadAsStringAsync().Result);

            return View((object)jsonResponse?.Property("html")?.Value.ToObject<string>());
        }
    }
}
