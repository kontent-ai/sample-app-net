using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverDancingGoatMVC.Models
{
    public class BrewerFilterViewModel
    {
        public bool Aerobie { get; set; }
        public bool Chemex { get; set; }
        public bool Espro { get; set; }
        public bool Hario { get; set; }


        public IEnumerable<string> GetManufacturerFilters()
        {
            if (Aerobie)
            {
                yield return "Aerobie";
            }
            if (Chemex)
            {
                yield return "Chemex";
            }
            if (Espro)
            {
                yield return "Espro";
            }
            if (Hario)
            {
                yield return "Hario";
            }
        }
    }
}