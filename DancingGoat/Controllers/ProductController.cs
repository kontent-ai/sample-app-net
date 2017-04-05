using KenticoCloud.Delivery;
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
            var response = await client.GetItemsAsync<object>(new EqualsFilter("elements.url_pattern", urlSlug), new InFilter("system.type", "brewer", "coffee"));

            if (response.Items.Count == 0)
            {
                throw new HttpException(404, "Not found");
            }
            else
            {
                ViewBag.FreeTasteRequested = TempData["formSubmited"] ?? false;
                ViewBag.UrlSlug = urlSlug;
                return View(response.Items[0]);
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