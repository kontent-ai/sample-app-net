using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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