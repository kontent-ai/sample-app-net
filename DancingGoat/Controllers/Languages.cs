using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DancingGoat.Controllers
{
    public class Languages : Controller
    {
        public IActionResult ChangeLanguage()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
