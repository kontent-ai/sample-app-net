using KenticoCloud.Delivery;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("about")]
    public class AboutController : AsyncController
    {
        private readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        [Route]
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemAsync("about_us");
            return View(response.Item);
        }
    }
}