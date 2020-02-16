using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class CoffeesViewModel
    {
        public IEnumerable<Coffee> Items { get; set; } = new List<Coffee>();
        public CoffeesFilterViewModel Filter { get; set; }
    }
}