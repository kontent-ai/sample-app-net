using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Models
{
    public class SelectProjectViewModel
    {
        public IEnumerable<ProjectModel> Projects { get; set; }

        [Required]
        [Display(Name = "Project ID")]
        public Guid? ProjectId { get; set; }

        [HiddenInput]
        public bool? ManualInput { get; set; }
    }
}