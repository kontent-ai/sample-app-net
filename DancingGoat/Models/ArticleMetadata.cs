using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    [MetadataType(typeof(ArticleMetadata))]
    public partial class Article
    {
    }

    public class ArticleMetadata
    {
        [DataType(DataType.Html)]
        public string BodyCopy { get; set; }
    }
}