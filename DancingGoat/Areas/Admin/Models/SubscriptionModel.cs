using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class SubscriptionModel
    {
        [JsonProperty("subscriptionId")]
        public Guid? SubscriptionId { get; set; }

        [JsonProperty("subscriptionRef")]
        public object subscriptionRef { get; set; }

        [JsonProperty("tags")]
        public object Tags { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        //[JsonProperty("accountId")]
        //public Guid? AccountId { get; set; }

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
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("nextBilling")]
        public object NextBilling { get; set; }

        [JsonProperty("isTrial")]
        public bool? IsTrial { get; set; }

        [JsonProperty("startAt")]
        public DateTime? StartAt { get; set; }

        [JsonProperty("endAt")]
        public DateTime? EndAt { get; set; }

        [JsonProperty("nextPeriodMaxUsers")]
        public object NextPeriodMaxUsers { get; set; }

        [JsonProperty("nextPeriodQuantity")]
        public object NextPeriodQuantity { get; set; }

        [JsonProperty("deliverRequestsLimit")]
        public object DeliverRequestsLimit { get; set; }

        [JsonProperty("deliverTrafficLimit")]
        public object DeliverTrafficLimit { get; set; }

        [JsonProperty("deliverStorageLimit")]
        public object DeliverStorageLimit { get; set; }

        [JsonProperty("deliverSlaLevel")]
        public object DeliverSlaLevel { get; set; }

        [JsonProperty("deliverSupportLevel")]
        public object DeliverSupportLevel { get; set; }
    }
}