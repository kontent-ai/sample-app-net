using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    public class PartnershipController : AsyncController
    {
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
