using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KenticoCloud.Delivery;

namespace DancingGoat.Models.ContentTypes
{
    public class Tweet
    {
        public string TweetLink { get; set; }

        public IEnumerable<MultipleChoiceOption> Theme { get; set; }

        public IEnumerable<MultipleChoiceOption> DisplayOptions { get; set; }
    }
}