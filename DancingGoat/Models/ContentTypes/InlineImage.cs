using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KenticoCloud.Delivery;

namespace DancingGoat.Models.ContentTypes
{
    public class InlineImage
    {
        public IEnumerable<Asset> Image { get; set; }

        public string Title { get; set; }

        public IEnumerable<MultipleChoiceOption> Flow { get; set; }
    }
}