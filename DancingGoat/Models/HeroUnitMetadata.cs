using System.ComponentModel.DataAnnotations;

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