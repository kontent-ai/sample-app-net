using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Errors")]
    public class ErrorsController : Controller
    {
        [LocalizedRoute("en-US", "NotFound")]
        public new ActionResult NotFound()
        {
            return View();
        }
    }
}