using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public Guid? ProjectGuid { get; set; }

        public DateTime? EndAt { get; set; }

        public bool NewlyGeneratedProject { get; set; }
    }
}
