using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KenticoCloud.Delivery;

namespace DancingGoat.Models.ContentTypes
{
    public class HostedVideo
    {
        public string VideoId { get; set; }

        public IEnumerable<MultipleChoiceOption> VideoHost { get; set; }
    } 
}