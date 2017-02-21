using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}