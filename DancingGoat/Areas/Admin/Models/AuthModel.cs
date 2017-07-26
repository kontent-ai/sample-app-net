using System;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class AuthModel
    {
        [JsonProperty("projectPermissions")]
        public object[] ProjectPermissions { get; set; }

        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("userId")]
        public Guid? UserId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("businessType")]
        public object BusinessType { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}