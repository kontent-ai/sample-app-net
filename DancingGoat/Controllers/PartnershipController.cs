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
        
        [Route]
        [HttpPost]
        public ActionResult Application()
        {
            TempData["formApplied"] = true;
            return RedirectToAction("Index");
        }
    }
}
