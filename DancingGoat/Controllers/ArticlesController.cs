using KenticoCloud.Delivery;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("articles")]
    public class ArticlesController : AsyncController
    {
        private readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemsAsync(
                new EqualsFilter("system.type", "article"),
                new OrderParameter("elements.post_date", SortOrder.Descending),
                new ElementsParameter("teaser_image", "post_date", "summary")
            );

            return View(response.Items);
        }

        [Route("{id}")]
        public async Task<ActionResult> Show(string id)
        {
            try
            {
                var response = await client.GetItemAsync(id);
                return View(response.Item);
            }
            catch (DeliveryException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new HttpException(404, "Not found");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}