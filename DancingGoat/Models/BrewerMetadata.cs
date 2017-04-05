using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    [MetadataType(typeof(BrewerMetadata))]
    public partial class Brewer
    {
    }

    public class BrewerMetadata
    {
        [DataType(DataType.Html)]
        public string ShortDescription { get; set; }

        [DataType(DataType.Html)]
        public string LongDescription { get; set; }
    }
}