using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DancingGoat.Areas.Admin.Models
{
    public class SignInViewModel
    {
        private const string EMAIL_VALIDATION_MESSAGE = "Incorrect e-mail format.";

        [Display(Name = "E-mail Address")]
        [Required]
        [EmailAddress(ErrorMessage = EMAIL_VALIDATION_MESSAGE)]
        [DataType(DataType.EmailAddress, ErrorMessage = EMAIL_VALIDATION_MESSAGE)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}