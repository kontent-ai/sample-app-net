using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using DancingGoat.Models;

using KenticoCloud.Personalization;
using KenticoCloud.Personalization.MVC;
using KenticoCloud.Delivery;

namespace DancingGoat.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly PersonalizationClient personalizationClient;

        public HomeController()
        {
            // Disable personalization when PersonalizationToken is not set
            var personalizationToken = ConfigurationManager.AppSettings["PersonalizationToken"];

            if (!string.IsNullOrWhiteSpace(personalizationToken))
            {
                personalizationClient = new PersonalizationClient(personalizationToken);
            }
        }

        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemAsync<Home>("home");

            var viewModel = new HomeViewModel
            {
                ContentItem = response.Item,
            };

            // Show promotion banner by default
            var showPromotion = true;

            if (personalizationClient != null)
            {
                // Get User ID of the current visitor
                var visitorUid = Request.GetCurrentPersonalizationUid();
                if (!string.IsNullOrEmpty(visitorUid))
                {
                    // Determine whether the visitor submitted a form
                    var visitorSubmittedForm = await personalizationClient.GetVisitorActionsAsync(visitorUid, ActionTypeEnum.FormSubmit);
                    showPromotion = !visitorSubmittedForm.Activity;
                }
            }
            var codeName = showPromotion ? "home_page_promotion" : "home_page_hero_unit";
            viewModel.Header = response.Item.HeroUnit.Cast<HeroUnit>().FirstOrDefault(x => x.System.Codename == codeName);

            return View(viewModel);
        }


        [ChildActionOnly]
        public ActionResult CompanyAddress()
        {
            var contact = Task.Run(() => client.GetItemAsync<Home>("home", new ElementsParameter("contact"))).Result.Item.Contact;

            return PartialView("CompanyAddress", contact);
        }

    }
}
