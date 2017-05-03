using KenticoCloud.Delivery;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ControllerBase
    {
        [Route("{urlSlug}")]
        public async Task<ActionResult> Detail(string urlSlug)
        {
            var item = (await client.GetItemsAsync<object>(new EqualsFilter("elements.url_pattern", urlSlug), new InFilter("system.type", "brewer", "coffee"))).Items.FirstOrDefault();

            if (item == null)
            {
                throw new HttpException(404, "Not found");
            }
            else
            {
                ViewBag.FreeTasteRequested = TempData["formSubmited"] ?? false;
                ViewBag.UrlSlug = urlSlug;
                return View(item.GetType().Name, item);
            }
        }

        [HttpPost]
        public ActionResult FreeTaste()
        {
            TempData["formSubmited"] = true;
            return RedirectToAction("Detail", new { urlSlug = Request.Form["product_url_slug"]});
        }
    }
}