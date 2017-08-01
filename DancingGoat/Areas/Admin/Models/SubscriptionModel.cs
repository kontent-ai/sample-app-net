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

        [JsonProperty("subscriptionName")]
        public string SubscriptionName { get; set; }

        [JsonProperty("subscriptionRef")]
        public string SubscriptionRef { get; set; }

        [JsonProperty("tags")]
        public string Tags { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("planName")]
        public string PlanName { get; set; }

        [JsonProperty("planDisplayName")]
        public string PlanDisplayName { get; set; }

        [JsonProperty("maxUsers")]
        public int MaxUsers { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusReason")]
        public string StatusReason { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("nextBilling")]
        public DateTime? NextBilling { get; set; }

        [JsonProperty("isTrial")]
        public bool IsTrial { get; set; }

        [JsonProperty("startAt")]
        public DateTime StartAt { get; set; }

        [JsonProperty("endAt")]
        public DateTime? EndAt { get; set; }

        [JsonProperty("nextPeriodMaxUsers")]
        public int? NextPeriodMaxUsers { get; set; }

        [JsonProperty("nextPeriodQuantity")]
        public int? NextPeriodQuantity { get; set; }

        [JsonProperty("deliverRequestsLimit")]
        public long? DeliverRequestsLimit { get; set; }

        [JsonProperty("deliverTrafficLimit")]
        public long? DeliverTrafficLimit { get; set; }

        [JsonProperty("deliverStorageLimit")]
        public long? DeliverStorageLimit { get; set; }

        [JsonProperty("deliverSlaLevel")]
        public string DeliverSlaLevel { get; set; }

        [JsonProperty("deliverSupportLevel")]
        public string DeliverSupportLevel { get; set; }

        [JsonProperty("billingStartAt")]
        public DateTime? BillingStartAt { get; set; }

        [JsonProperty("nextPeriodStartAt")]
        public DateTime? NextPeriodStartAt { get; set; }

        [JsonProperty("fsSubscriptionRef")]
        public string FsSubscriptionRef { get; set; }

        [JsonProperty("currentPlanId")]
        public Guid? CurrentPlanId { get; set; }
    }
}