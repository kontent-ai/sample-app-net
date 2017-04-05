using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    [MetadataType(typeof(HomeMetadata))]
    public partial class Home
    {
    }

    public class HomeMetadata
    {
        [DataType(DataType.Html)]
        public string Contact { get; set; }
    }
}