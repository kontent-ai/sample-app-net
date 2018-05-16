using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class AboutUsViewModel : AboutUs
    {
        public string ItemId { get; set; }
        public string Language { get; set; }
        public IList<FactAboutUsViewModel> FactViewModels { get; set; }
    }
}