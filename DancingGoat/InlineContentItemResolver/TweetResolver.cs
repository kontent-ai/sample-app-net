using System;
using System.Linq;
using System.Net.Http;
using DancingGoat.Models.ContentTypes;
using KenticoCloud.Delivery.InlineContentItems;
using Newtonsoft.Json.Linq;

namespace DancingGoat.InlineContentItemResolver
{
    public class TweetResolver : IInlineContentItemsResolver<Tweet>
    {
        private static readonly HttpClient Client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };

        public string Resolve(ResolvedContentItemData<Tweet> data)
        {
            var selectedTheme = data.Item.Theme.FirstOrDefault()?.Name?.ToLower() ?? "light";
            var displayOptions = data.Item.DisplayOptions.ToList();
            var hideThread = displayOptions.Any(o => o.Codename.Equals("hide_thread"));
            var hideMedia = displayOptions.Any(o => o.Codename.Equals("hide_media"));
            var options = $"&hide_thread={hideThread}&hide_media={hideMedia}";
            var tweetLink = data.Item.TweetLink;

            var tweetResponse = Client.GetAsync(
                $"https://publish.twitter.com/oembed?url={tweetLink}&theme={selectedTheme}" + options).Result;
            dynamic jsonResponse = JObject.Parse(tweetResponse.Content.ReadAsStringAsync().Result);

            return "<div class=\"tweet__wrapper\">" + jsonResponse.html + "</div>";
        }
    }
}