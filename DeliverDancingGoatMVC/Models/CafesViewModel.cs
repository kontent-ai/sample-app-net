using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using KenticoCloud.Deliver;

namespace DeliverDancingGoatMVC.Models
{
    public class CafesViewModel
    {
        public List<ContentItem> CompanyCafes { get; set; }

        public List<ContentItem> PartnerCafes { get; set; }
    }
}