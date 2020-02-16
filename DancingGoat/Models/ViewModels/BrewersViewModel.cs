using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class BrewersViewModel
    {
        public IEnumerable<Brewer> Items { get; set; }

        public BrewerFilterViewModel Filter { get; set; }
    }
}