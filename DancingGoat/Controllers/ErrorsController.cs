using DancingGoat.Configuration;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    [LocalizedRoute(CultureConstants.EnglishCulture, "Errors")]
    public class ErrorsController : Controller
    {
        [LocalizedRoute(CultureConstants.EnglishCulture, "NotFound")]
        public new ActionResult NotFound()
        {
            return View();
        }
    }
}