using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Models
{
    public class AboutUsViewModel : AboutUs
    {
        public IList<FactAboutUsViewModel> FactViewModels { get; set; }
    }
}