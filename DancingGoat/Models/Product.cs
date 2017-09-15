using KenticoCloud.Delivery;
using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class Product
    {
        public virtual ContentItemSystemAttributes ProductSystem { get; set; }
        public virtual string ProductProductName { get; set; }
        public virtual decimal? ProductPrice { get; set; }
        public virtual IEnumerable<Asset> ProductImage { get; set; }
        public virtual IEnumerable<TaxonomyTerm> ProductProductStatus { get; set; }
        public virtual IRichTextContent ProductShortDescription { get; set; }
        public virtual IRichTextContent ProductLongDescription { get; set; }
        public virtual string ProductUrlPattern { get; set; }
    }
}