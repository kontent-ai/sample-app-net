using Kentico.Kontent.Delivery;
using System.Collections.Generic;

namespace DancingGoat.Models
{
    public interface IProduct : IMetadata
    {   
        ContentItemSystemAttributes ProductSystem { get; set; }
        string ProductProductName { get; set; }
        decimal? ProductPrice { get; set; }
        IEnumerable<Asset> ProductImage { get; set; }
        IEnumerable<TaxonomyTerm> ProductProductStatus { get; set; }
        IRichTextContent ProductShortDescription { get; set; }
        IRichTextContent ProductLongDescription { get; set; }
        string ProductUrlPattern { get; set; }
    }
}