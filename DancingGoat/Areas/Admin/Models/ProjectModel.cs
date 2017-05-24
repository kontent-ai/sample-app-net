using System;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class ProjectModel
    {
        [JsonProperty(PropertyName = "projectGuid")]
        public Guid? ProjectGuid { get; set; }

        [JsonProperty(PropertyName = "projectName")]
        public string ProjectName { get; set; }

        [JsonProperty(PropertyName = "projectType")]
        public string ProjectType { get; set; }

        [JsonProperty(PropertyName = "inactive")]
        public bool? Inactive { get; set; }

        [JsonProperty(PropertyName = "deactivateAt")]
        public DateTime? DeactivatedAt { get; set; }

        [JsonProperty(PropertyName = "activateAt")]
        public DateTime? ActivatedAt { get; set; }
    }
}