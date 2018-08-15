using System;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KenticoCloud.Recommender.SDK;

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

                    var recommendationClient = new RecommendationClient("https://localhost:44342",
                        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiJ1c3JfMHZPNHpxS2NiQ1dBdTdhMzM3ekxveiIsInBpZCI6IjNkN2MxMWU3LTEzNmItMDAzMS04OTljLTc5MTE5Zjg3MTdhMyIsImp0aSI6IklncFgzQlpINWVkWks0emsiLCJhdWQiOiJodHRwczovL2VuZ2FnZS1hcGkua2VudGljb2Nsb3VkLmNvbS8ifQ.YeDnsP7B8NGYn3dOSJGwu3x3o0QDjk6RinkWtGqiZ-o",
                        2);

                    var lastMonth = 24 * 60 * 60 * 30;

                    var recommendedArticles = await recommendationClient.GetRecommendationsAsync(article.System.Codename, Request, Response, 
                        filterQuery: "\"personas=Barista\" in 'properties'", 
                        boosterQuery:$"if 'lastupdated' >= now() - {lastMonth} then 2 else 1", 
                        limit: 2);
                    var articles = (await client.GetItemsAsync<Article>(new InFilter("system.codename", recommendedArticles.Select(a => a.Codename).ToArray()))).Items;

                    article.RelatedArticles = articles.Select(a => (object) a);
                    return View(article);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}