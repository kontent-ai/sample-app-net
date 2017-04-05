using KenticoCloud.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DancingGoat.Models
{
    public partial class Brewer : Product
    {
        public override ContentItemSystemAttributes ProductSystem
        {
            get { return System; }
            set { System = value; }
        }
        public override string ProductProductName
        {
            get { return ProductName; }
            set { ProductName = value; }
        }
        public override decimal? ProductPrice
        {
            get { return Price; }
            set { Price = value; }
        }
        public override IEnumerable<Asset> ProductImage
        {
            get { return Image; }
            set { Image = value; }
        }
        public override IEnumerable<TaxonomyTerm> ProductProductStatus
        {
            get { return ProductStatus; }
            set { ProductStatus = value; }
        }
        public override string ProductShortDescription
        {
            get { return ShortDescription; }
            set { ShortDescription = value; }
        }
        public override string ProductLongDescription
        {
            get { return LongDescription; }
            set { LongDescription = value; }
        }
        public override string ProductUrlPattern
        {
            get { return UrlPattern; }
            set { UrlPattern = value; }
        }
    }
}