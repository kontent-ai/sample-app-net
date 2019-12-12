using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class BrewerFilterViewModel
    {
        public IList<SelectListItem> AvailableManufacturers { get; set; } = new List<SelectListItem>();
        public IList<SelectListItem> AvailableProductStatuses { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> GetFilteredManufacturers()
            => AvailableManufacturers.Where(x => x.Selected).Select(x => x.Value);

        public IEnumerable<string> GetFilteredProductStatuses()
            => AvailableProductStatuses.Where(x => x.Selected).Select(x => x.Value);

    }
}