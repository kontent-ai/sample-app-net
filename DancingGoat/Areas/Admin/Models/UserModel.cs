using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Areas.Admin.Models
{
    public class UserModel
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("inactive")]
        public bool Inactive { get; set; }

        [JsonProperty("businessType")]
        public string BusinessType { get; set; }

        [JsonProperty("isInvitationPending")]
        public bool IsInvitationPending { get; set; }

        [JsonProperty("invitationGuid")]
        public Guid? InvitationGuid { get; set; }

        [JsonProperty("invitationExpiresAt")]
        public DateTime? InvitationExpiresAt { get; set; }

        [JsonProperty("administratedSubscriptions")]
        public IEnumerable<AdministeredSubscriptionModel> AdministeredSubscriptions { get; set; }
    }
}