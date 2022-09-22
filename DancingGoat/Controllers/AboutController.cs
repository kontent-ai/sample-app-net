using System.Collections.Generic;
using System.Threading.Tasks;
using DancingGoat.Models;
using Kontent.Ai.Delivery.Abstractions;
using Kontent.Ai.Management.Helpers.Models;
using Kontent.Ai.Urls.Delivery.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace DancingGoat.Controllers
{
    public class AboutController : ControllerBase
    {
        public AboutController(IDeliveryClientFactory deliveryClientFactory) : base(deliveryClientFactory)
        {
        }
        
        public async Task<ActionResult> Index()
        {
            var response = await _client.GetItemAsync<AboutUs>("about_us", new LanguageParameter(Language));

            var viewModel = new AboutUsViewModel
            {
                ItemId = response.Item.System.Id,
                Language = response.Item.System.Language,
                FactViewModels = MapFactsAboutUs(response)
            };

            return View(viewModel);
        }

        private IList<FactAboutUsViewModel> MapFactsAboutUs(IDeliveryItemResponse<AboutUs> response)
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