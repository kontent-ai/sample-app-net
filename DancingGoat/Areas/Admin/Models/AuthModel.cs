using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class AuthModel
    {
        [JsonProperty(PropertyName = "projectPermissions")]
        public object[] ProjectPermissions { get; set; }

        [JsonProperty(PropertyName = "identity")]
        public string Identity { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "businessType")]
        public object BusinessType { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}