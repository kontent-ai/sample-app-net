using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class AboutUsViewModel : AboutUs
    {
        public IList<FactAboutUsViewModel> FactViewModels { get; set; }
    }
}