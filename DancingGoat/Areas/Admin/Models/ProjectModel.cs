using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class ProjectModel
    {
        [JsonProperty("projectGuid")]
        public Guid? ProjectId { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("projectType")]
        public string ProjectType { get; set; }

        [JsonProperty("inactive")]
        public bool? Inactive { get; set; }

        [JsonProperty("deactivateAt")]
        public DateTime? DeactivatedAt { get; set; }

        [JsonProperty("activateAt")]
        public DateTime? ActivatedAt { get; set; }

        [JsonProperty("subscriptionId")]
        public Guid? SubscriptionId { get; set; }
    }
}