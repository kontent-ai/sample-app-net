using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    [LocalizedRoute("en-US", "Partner")]
    [LocalizedRoute("es-ES", "Afiliados")]
    public class PartnerShipController : Controller
    {
        [LocalizedRoute("en-US", "Index")]
        [LocalizedRoute("es-ES", "Index")]
        public ActionResult Index()
        {
            ViewBag.PartnershipRequested = TempData["formApplied"] ?? false;
            return View();
        }

        /// <summary>
        /// Dummy action.
        /// </summary>
        [HttpPost]
        public ActionResult Application()
        {
            // If needed, put your code here to work with the uploaded data in MVC.
            TempData["formApplied"] = true;
            return RedirectToAction("Index");
        }
    }
}