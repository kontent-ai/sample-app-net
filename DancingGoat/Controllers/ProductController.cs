using KenticoCloud.Delivery;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : AsyncController
    {
        private readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        [Route("{id}")]
        public async Task<ActionResult> Detail(string id)
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