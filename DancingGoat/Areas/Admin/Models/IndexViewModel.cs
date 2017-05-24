using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public AuthUpViewModel AuthModel { get; set; }
        public ProjectIdUpViewModel ProjectIdModel { get; set; }
    }
}