using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DancingGoat.Models
{
    public class BrewerFilterViewModel
    {
        public IList<SelectListItem> AvailableManufacturers { get; set; } = new List<SelectListItem>();
        public IList<SelectListItem> AvailableStatusTypes { get; set; } = new List<SelectListItem>();

        public IEnumerable<string> GetFilteredManufacturers()
            => AvailableManufacturers.Where(x => x.Selected).Select(x => x.Value);

        public IEnumerable<string> GetFilteredStatusTypes()
            => AvailableStatusTypes.Where(x => x.Selected).Select(x => x.Value);

    }
}