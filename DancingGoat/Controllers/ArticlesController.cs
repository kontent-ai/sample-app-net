using System;
using System.Linq;
using System.Threading.Tasks;
using DancingGoat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Recommender;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Articles")]
    [LocalizedRoute("es-ES", "Artículos")]
    public class ArticlesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArticlesController(IConfiguration configuration, IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
            _configuration = configuration;
        }

        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemsAsync<Article>(
                new EqualsFilter("system.type", "article"),
                new OrderParameter("elements.post_date", SortOrder.Descending),
                new ElementsParameter("teaser_image", "post_date", "summary", "url_pattern", "title"),
                new LanguageParameter(Language)
            );

            return View(response.Items);
        }

        [LocalizedRoute("en-US", "Detail")]
        [LocalizedRoute("es-ES", "Detail")]
        public async Task<ActionResult> Show(string urlSlug)
        {
            var response = await _client.GetItemsAsync<Article>(
                new EqualsFilter("elements.url_pattern", urlSlug),
                new EqualsFilter("system.type", "article"),
                new DepthParameter(1),
                new LanguageParameter(Language)
            );

            if (response.Items.Count == 0)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    var article = response.Items[0];
                    var recommendationApiKey = _configuration.GetValue<string>("RecommendationApiKey");

                    // If the recommender API key is present
                    if (!string.IsNullOrWhiteSpace(recommendationApiKey))
                    {
                        /* Get recommendations from the Recommendation engine */
                        var recommendationClient = new RecommendationClient(recommendationApiKey, 5);
                        var lastMonth = TimeSpan.FromDays(30).Milliseconds;

                        var recommendedArticles = await recommendationClient
                            .CreateRequest(Request, Response, codename: article.System.Codename, limit: 2, contentType: article.System.Type)
                            //.WithFilterQuery("\"personas=Barista\" in 'properties'")
                            //.WithBoosterQuery($"if 'lastupdated' >= now() - {lastMonth} then 2 else 1")
                            .Execute();

                        var articles = (await _client.GetItemsAsync<Article>(new InFilter("system.codename", recommendedArticles.Select(a => a.Codename).ToArray()))).Items;
                        article.RelatedArticles = articles.Select(a => (object)a);
                        return View(article);
                    }
                    else
                    {
                        return View(article);
                    }
                }
                catch (Exception)
                {
                    return View(response.Items[0]);
                }
            }
        }
    }
}