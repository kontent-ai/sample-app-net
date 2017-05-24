using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class SubscriptionModel
    {
        [JsonProperty(PropertyName = "subscriptionId")]
        public Guid? SubscriptionId { get; set; }

        [JsonProperty(PropertyName = "subscriptionRef")]
        public object subscriptionRef { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public object Tags { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        //[JsonProperty(PropertyName = "accountId")]
        //public Guid? AccountId { get; set; }

        [JsonProperty(PropertyName = "planName")]
        public string PlanName { get; set; }

        [JsonProperty(PropertyName = "planDisplayName")]
        public string PlanDisplayName { get; set; }

        [JsonProperty(PropertyName = "maxUsers")]
        public int MaxUsers { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "statusReason")]
        public string StatusReason { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty(PropertyName = "nextBilling")]
        public object NextBilling { get; set; }

        [JsonProperty(PropertyName = "isTrial")]
        public bool? IsTrial { get; set; }

        [JsonProperty(PropertyName = "startAt")]
        public DateTime? StartAt { get; set; }

        [JsonProperty(PropertyName = "endAt")]
        public DateTime? EndAt { get; set; }

        [JsonProperty(PropertyName = "nextPeriodMaxUsers")]
        public object NextPeriodMaxUsers { get; set; }

        [JsonProperty(PropertyName = "nextPeriodQuantity")]
        public object NextPeriodQuantity { get; set; }

        [JsonProperty(PropertyName = "deliverRequestsLimit")]
        public object DeliverRequestsLimit { get; set; }

        [JsonProperty(PropertyName = "deliverTrafficLimit")]
        public object DeliverTrafficLimit { get; set; }

        [JsonProperty(PropertyName = "deliverStorageLimit")]
        public object DeliverStorageLimit { get; set; }

        [JsonProperty(PropertyName = "deliverSlaLevel")]
        public object DeliverSlaLevel { get; set; }

        [JsonProperty(PropertyName = "deliverSupportLevel")]
        public object DeliverSupportLevel { get; set; }
    }
}