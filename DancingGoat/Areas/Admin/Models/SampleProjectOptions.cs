using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DancingGoat.Areas.Admin.Models
{
    public class SampleProjectOptions
    {
        [JsonProperty("projectName")]
        public string ProjectName { get; set; }
    }
}
