using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverDancingGoatMVC.Models
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
                yield return "Washed";
            }
            if (SemiWashed)
            {
                yield return "Semi-washed";
            }
            if (Natural)
            {
                yield return "Natural";
            }
        }
    }
}