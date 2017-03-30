using KenticoCloud.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Models
{
    public class Product
    {
        public virtual ContentItemSystemAttributes ProductSystem { get; set; }
        public virtual string ProductProductName { get; set; }
        public virtual decimal? ProductPrice { get; set; }
        public virtual IEnumerable<Asset> ProductImage { get; set; }
        public virtual IEnumerable<TaxonomyTerm> ProductProductStatus { get; set; }
        public virtual string ProductShortDescription { get; set; }
        public virtual string ProductLongDescription { get; set; }
    }
}