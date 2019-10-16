using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using KenticoCloud.ContentManagement.Helpers.Models;

namespace DancingGoat.Controllers
{
    public class AboutController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemAsync<AboutUs>("about_us");

            var viewModel = new AboutUsViewModel
            {
                ItemId = response.Item.System.Id,
                Language = response.Item.System.Language,
                OurMotto = response.Item.OurMotto,
                Video = response.Item.Video,
                FactViewModels = MapFactsAboutUs(response)
            };

            return View(viewModel);
        }

        private IList<FactAboutUsViewModel> MapFactsAboutUs(DeliveryItemResponse<AboutUs> response)
        {
            var facts = new List<FactAboutUsViewModel>();

            if (response.Item == null)
            {
                return facts;
            }

            int i = 0;

            foreach (var fact in response.Item.Facts)
            {
                var factViewModel = new FactAboutUsViewModel
                {
                    Fact = (FactAboutUs)fact,
                    ParentItemElementIdentifier = new ElementIdentifier(response.Item.System.Id, AboutUs.FactsCodename)
                };

                if (i++ % 2 == 0)
                {
                    factViewModel.Odd = true;
                }

                facts.Add(factViewModel);
            }

            return facts;
        }
    }
}
