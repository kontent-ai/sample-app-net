using Kentico.Kontent.Delivery.Abstractions;
using System.Collections.Generic;

namespace DancingGoat.Models
{
    public interface IProduct : IMetadata
    {
        string ProductNameElementCodename { get; }
        string ProductStatusElementCodename { get; }
        string ProductImageElementCodename { get; }
        string ProductPriceElementCodename { get; }
        string ProductShortDescriptionCodename { get; }
        string ProductLongDescriptionCodename { get; }
        IContentItemSystemAttributes ProductSystem { get; set; }
        string ProductProductName { get; set; }
        decimal? ProductPrice { get; set; }
        IEnumerable<IAsset> ProductImage { get; set; }
        IEnumerable<ITaxonomyTerm> ProductProductStatus { get; set; }
        IRichTextContent ProductShortDescription { get; set; }
        IRichTextContent ProductLongDescription { get; set; }
        string ProductUrlPattern { get; set; }
        public IContentItemSystemAttributes System { get; set; }
    }
}