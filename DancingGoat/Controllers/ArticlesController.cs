using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Recommender;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class ArticlesController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync<Article>(
                new EqualsFilter("system.type", "article"),
                new OrderParameter("elements.post_date", SortOrder.Descending),
                new ElementsParameter("teaser_image", "post_date", "summary", "url_pattern", "title")
            );

            return View(response.Items);
        }

        public async Task<ActionResult> Show(string urlSlug)
        {
            var response = await client.GetItemsAsync<Article>(
                new EqualsFilter("elements.url_pattern", urlSlug),
                new EqualsFilter("system.type", "article"),
                new DepthParameter(1)
            );

            if (response.Items.Count == 0)
            {
                throw new HttpException(404, "Not found");
            }
            else
            {
                try
                {
                    var article = response.Items[0];
                    var recommendationApiKey = ConfigurationManager.AppSettings["RecommendationApiKey"];

                    // If the recommender API key is present
                    if (!string.IsNullOrWhiteSpace(recommendationApiKey))
                    {
                        /* Get recommendations from the Recommendation engine */
                        var recommendationClient = new Kentico.Kontent.Recommender.MVC.RecommendationClient(recommendationApiKey, 5);
                        var lastMonth = TimeSpan.FromDays(30).Milliseconds;

                        var recommendedArticles = await recommendationClient
                            .CreateRequest(Request, Response, codename: article.System.Codename, limit: 2, contentType: article.System.Type)
                            //.WithFilterQuery("\"personas=Barista\" in 'properties'")
                            //.WithBoosterQuery($"if 'lastupdated' >= now() - {lastMonth} then 2 else 1")
                            .Execute();

                        var articles = (await client.GetItemsAsync<Article>(new InFilter("system.codename", recommendedArticles.Select(a => a.Codename).ToArray()))).Items;
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