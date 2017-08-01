using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Models
{
    public class AdminHelperResults
    {
        public DateTime? EndAt { get; set; }
        public string ViewName { get; set; }
        public object Model { get; set; }
    }
}