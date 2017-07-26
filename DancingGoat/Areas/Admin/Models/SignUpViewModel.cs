using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Models
{
    public class SignUpViewModel
    {
        [Display(Name = "First name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect e-mail format.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "")]
        //[Required]
        //public bool? TermsAccepted { get; set; } = false;
    }
}