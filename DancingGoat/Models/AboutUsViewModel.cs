using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class AboutUsViewModel : AboutUs
    {
        public AboutUs AboutUs { get; set; }
        public IList<FactAboutUsViewModel> FactViewModels { get; set; }
    }
}