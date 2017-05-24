using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DancingGoat.Areas.Admin.Models
{
    public class ProjectIdUpViewModel
    {
        [Display(Name = "Custom Project ID")]
        public string OwnProjectGuid { get; set; }
    }
}