using System;

namespace DancingGoat.Areas.Admin.Models
{
    public class AdminHelperResults
    {
        public DateTime? EndAt { get; set; }
        public string ViewName { get; set; }
        public object Model { get; set; }
    }
}