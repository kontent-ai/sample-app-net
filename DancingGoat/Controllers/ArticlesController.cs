using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("articles")]
    public class ArticlesController : ControllerBase
    {
        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync<Article>(
                new EqualsFilter("system.type", "article"),
                new OrderParameter("elements.post_date", SortOrder.Descending),
                new ElementsParameter("teaser_image", "post_date", "summary", "url_pattern")
            );

            return View(response.Items);
        }

        [Route("{urlSlug}")]
        public async Task<ActionResult> Show(string urlSlug)
        {
            var response = await client.GetItemsAsync<Article>(new EqualsFilter("elements.url_pattern", urlSlug), new EqualsFilter("system.type", "article"));

            if (response.Items.Count == 0)
            {
                throw new HttpException(404, "Not found");
            }
            else
            {
                return View(response.Items[0]);
            }
        }
    }
}