using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class ErrorsController : Controller
    {
        public new ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}