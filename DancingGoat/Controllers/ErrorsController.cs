using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class ErrorsController : Controller
    {
        public new ActionResult NotFound()
        {
            return View();
        }
    }
}