using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Areas.Admin.Models
{
    public class IndexViewModel
    {
        public SignInViewModel SignInModel { get; set; }
        public SignUpViewModel SignUpModel { get; set; }
        public SelectProjectViewModel SelectProjectModel { get; set; }
    }
}