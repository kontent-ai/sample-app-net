using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kontent.Ai.Delivery.Abstractions;
using DancingGoat.Configuration;
using Kontent.Ai.Urls.Delivery.QueryParameters;
using Kontent.Ai.Urls.Delivery.QueryParameters.Filters;

namespace DancingGoat.Controllers
{
    [LocalizedRoute(Constants.EnglishCulture, "Articles")]
    [LocalizedRoute(Constants.SpanishCulture, "Articulos")]
    public class ArticlesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArticlesController(IConfiguration configuration, IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
            _configuration = configuration;
        }

        [LocalizedRoute(Constants.EnglishCulture, "")]
        [LocalizedRoute(Constants.SpanishCulture, "")]
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Article>(
                new OrderParameter("elements.post_date", SortOrder.Descending),
                new ElementsParameter(Article.TeaserImageCodename, Article.PostDateCodename, Article.SummaryCodename, Article.UrlPatternCodename, Article.TitleCodename),
                new LanguageParameter(Language)
            );

            return View(response.Items);
        }

        [LocalizedRoute(Constants.EnglishCulture, "Show")]
        [LocalizedRoute(Constants.SpanishCulture, "Show")]
        public async Task<ActionResult> Show(string urlSlug)
        {
            var response = await _client.GetItemsAsync<Article>(
                new EqualsFilter($"elements.{Article.UrlPatternCodename}", urlSlug),
                new DepthParameter(1),
                new LanguageParameter(Language)
            );

            if (response.Items.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return View(response.Items[0]);
            }
        }
    }
}