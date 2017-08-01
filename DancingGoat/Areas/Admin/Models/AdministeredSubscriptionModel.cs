using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class AdministeredSubscriptionModel
    {
        [JsonProperty("subscriptionId")]
        public Guid SubscriptionId { get; set; }

        [JsonProperty("subscriptionName")]
        public string SubscriptionName { get; set; }

        [JsonProperty("isExpired")]
        public bool IsExpired { get; set; }
    }
}