using System.Collections.Generic;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Models
{
    public interface IMetadata
    {
        string MetadataMetaTitle { get; set; }
        string MetadataMetaDescription { get; set; }
        string MetadataOgTitle { get; set; }
        string MetadataOgDescription { get; set; }
        IEnumerable<Asset> MetadataOgImage { get; set; }
        string MetadataTwitterSite { get; set; }
        string MetadataTwitterCreator { get; set; }
        string MetadataTwitterTitle { get; set; }
        string MetadataTwitterDescription { get; set; }
        IEnumerable<Asset> MetadataTwitterImage { get; set; }
    }
}