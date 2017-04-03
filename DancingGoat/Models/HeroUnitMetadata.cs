using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DancingGoat.Models
{
    [MetadataType(typeof(HeroUnitMetadata))]
    public partial class HeroUnit
    {
    }

    public class HeroUnitMetadata
    {
        [DataType(DataType.Html)]
        public string MarketingMessage { get; set; }
    }
}