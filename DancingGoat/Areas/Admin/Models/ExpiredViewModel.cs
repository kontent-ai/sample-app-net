using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Areas.Admin.Models
{
    public class ExpiredViewModel
    {
        public string Token { get; set; }
        public DateTime? EndAt { get; set; }
    }
}