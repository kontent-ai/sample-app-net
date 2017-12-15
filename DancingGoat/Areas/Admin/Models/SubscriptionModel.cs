using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class SubscriptionModel
    {
        [JsonProperty("projects")]
        public IEnumerable<SubscriptionProjectModel> Projects { get; set; }

        [JsonProperty("currentPlan")]
        public PlanModel CurrentPlan { get; set; }

        [JsonProperty("subscriptionId")]
        public Guid SubscriptionId { get; set; }

        [JsonProperty("planName")]
        public string PlanName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("endAt")]
        public DateTime? EndAt { get; set; }
    }
}