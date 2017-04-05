using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    [MetadataType(typeof(CoffeeMetadata))]
    public partial class Coffee
    {
    }

    public class CoffeeMetadata
    {
        [DataType(DataType.Html)]
        public string ShortDescription { get; set; }

        [DataType(DataType.Html)]
        public string LongDescription { get; set; }
    }
}