using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class PlanModel
    {
        [JsonProperty("planName")]
        public string Name { get; set; }

        [JsonProperty("planDisplayName")]
        public string DisplayName { get; set; }

        [JsonProperty("startAt")]
        public DateTime StartAt { get; set; }

        [JsonProperty("endAt")]
        public DateTime? EndAt { get; set; }
    }
}