using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class CoffeesFilterViewModel
    {
        public bool Washed { get; set; }
        public bool SemiWashed { get; set; }
        public bool Natural { get; set; }

        public IEnumerable<string> GetFilteredValues()
        {
            if (Washed)
            {
                yield return "wet__washed_";
            }
            if (SemiWashed)
            {
                yield return "semi_dry";
            }
            if (Natural)
            {
                yield return "dry__natural_";
            }
        }
    }
}