using System.Collections.Generic;
using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Models
{
    public partial class Coffee : IProduct, IMetadata, IDetailItem
    {
        public IContentItemSystemAttributes ProductSystem
        {
            get { return System; }
            set { System = value; }
        }
        public string ProductProductName
        {
            get { return ProductName; }
            set { ProductName = value; }
        }
        public decimal? ProductPrice
        {
            get { return Price; }
            set { Price = value; }
        }
        public IEnumerable<IAsset> ProductImage
        {
            get { return Image; }
            set { Image = value; }
        }
        public IEnumerable<ITaxonomyTerm> ProductProductStatus
        {
            get { return ProductStatus; }
            set { ProductStatus = value; }
        }
        public IRichTextContent ProductShortDescription
        {
            get { return ShortDescription; }
            set { ShortDescription = value; }
        }
        public IRichTextContent ProductLongDescription
        {
            get { return LongDescription; }
            set { LongDescription = value; }
        }
        public string ProductUrlPattern
        {
            get { return UrlPattern; }
            set { UrlPattern = value; }
        }
        public string Type => System.Type;
        public string Id => System.Id;
    }
}