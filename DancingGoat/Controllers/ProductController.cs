using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ControllerBase
    {
        [Route("{id}")]
        public async Task<ActionResult> Detail(string id)
        {
            try
            {
                var response = await client.GetItemAsync<object>(id);
                ViewBag.FreeTasteRequested = TempData["formSubmited"] ?? false;
                ViewBag.Id = id;
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

        [HttpPost]
        public ActionResult FreeTaste()
        {
            TempData["formSubmited"] = true;
            return RedirectToAction("Detail", new { id = Request.Form["product_id"]});
        }
    }
}