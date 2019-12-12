using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DancingGoat.Models
{
    public class CoffeesFilterViewModel
    {
        public IList<SelectListItem> AvailableProcessings { get; set; } = new List<SelectListItem>();
        public IList<SelectListItem> AvailableProductStatuses { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> GetFilteredProcessings()
            => AvailableProcessings.Where(x => x.Selected).Select(x => x.Value);

        public IEnumerable<string> GetFilteredProductStatuses()
            => AvailableProductStatuses.Where(x => x.Selected).Select(x => x.Value);
    }
}