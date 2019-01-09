using System;
using System.Configuration;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KenticoCloud.Recommender;
using KenticoCloud.Recommender.MVC;

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
                    const string key = "ew0KICAiYWxnIjogIkhTMjU2IiwNCiAgInR5cCI6ICJKV1QiDQp9.ew0KICAidWlkIjogImViZjQxN2VjLTYzN2QtMDBjMC1hZmNmLTJjYmJlMWMwODM5OCIsDQogICJwaWQiOiAiZWJmNDE3ZWMtNjM3ZC0wMGMwLWFmY2YtMmNiYmUxYzA4Mzk4IiwNCiAgImp0aSI6ICJRSzI3RHZLaENiNExad2Q3IiwNCiAgImF1ZCI6ICJrYy1yZWNvbW1lbmRlci1hcGktYmV0YS5rZW50aWNvY2xvdWQuY29tIg0KfQ.xM568KPKuiRjjyk-TlP3GV_igyAQbLi09ar385JxN3g";
                    var article = response.Items[0];
                    
                    /* Get recommendations from the Recommendation engine */
                    var recommendationClient = new RecommendationClient(key, 5);
                    var lastMonth = TimeSpan.FromDays(30).Milliseconds;

                    var recommendedArticles = await recommendationClient
                        .CreateRequest(Request, Response, codename: article.System.Codename, limit: 2, contentType: article.System.Type)
                        //.WithFilterQuery("\"personas=Barista\" in 'properties'")
                        //.WithBoosterQuery($"if 'lastupdated' >= now() - {lastMonth} then 2 else 1")
                        .Execute();

                    var articles = (await client.GetItemsAsync<Article>(new InFilter("system.codename", recommendedArticles.Select(a => a.Codename).ToArray()))).Items;
                    article.RelatedArticles = articles.Select(a => (object) a);
                    return View(article);
                }
                catch (Exception)
                {
                    return View(response.Items[0]);
                }
            }
        }
    }
}