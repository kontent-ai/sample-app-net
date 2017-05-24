using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [RoutePrefix("partnership")]
    public class PartnershipController : AsyncController
    {
        [Route]
        public ActionResult Index()
        {
            ViewBag.PartnershipRequested = TempData["formApplied"] ?? false;
            return View();
        }
        
        /// <summary>
        /// Dummy action; form information is being handed over to Kentico Cloud Engagement management service through JavaScript.
        /// </summary>
        [Route]
        [HttpPost]
        public ActionResult Application()
        {
            // If needed, put your code here to work with the uploaded data in MVC.
            TempData["formApplied"] = true;
            return RedirectToAction("Index");
        }
    }
}
