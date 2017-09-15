using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class SubscriptionProjectModel
    {
        [JsonProperty("projectId")]
        public Guid Id { get; set; }

        [JsonProperty("projectName")]
        public string Name { get; set; }
    }
}