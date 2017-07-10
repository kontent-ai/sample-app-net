using System.Threading.Tasks;
using System.Web.Mvc;
using DancingGoat.Models;
using System.Collections.Generic;

namespace DancingGoat.Controllers
{
    public class AboutController : ControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var response = await client.GetItemAsync<AboutUs>("about_us");

            var viewModel = new AboutUsViewModel
            {
                FactViewModels = new List<FactAboutUsViewModel>()
            };

            int i = 0;

            foreach (var fact in response.Item?.Facts)
            {
                var factViewModel = new FactAboutUsViewModel
                {
                    Fact = (FactAboutUs)fact
                };

                if (i++ % 2 == 0)
                {
                    factViewModel.Odd = true;
                }

                viewModel.FactViewModels.Add(factViewModel);
            }

            return View(viewModel);
        }
    }
}