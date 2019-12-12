using System.Collections.Generic;
using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Abstractions;
using DancingGoat.Models;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Management.Helpers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DancingGoat.Controllers
{
    public class AboutController : ControllerBase
    {
        public AboutController(IOptionsSnapshot<DeliveryOptions> deliveryOptions, IAppSettingProvider settingProvider, IDeliveryClient client) : base(deliveryOptions, settingProvider, client)
        {
        }

        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemAsync<AboutUs>("about_us");

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