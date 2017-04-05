using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    [MetadataType(typeof(FactAboutUsMetadata))]
    public partial class FactAboutUs
    {
    }

    public class FactAboutUsMetadata
    {
        [DataType(DataType.Html)]
        public string Description { get; set; }
    }
}