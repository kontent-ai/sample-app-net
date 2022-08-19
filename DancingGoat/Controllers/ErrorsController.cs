using DancingGoat.Configuration;
using AspNetCore.Mvc.Routing.Localization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    [LocalizedRoute(Constants.EnglishCulture, "Errors")]
    public class ErrorsController : Controller
    {
        [LocalizedRoute(Constants.EnglishCulture, "NotFound")]
        public new ActionResult NotFound()
        {
            return View();
        }
    }
}