using KenticoCloud.Deliver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliverDancingGoatMVC.Models
{
    public class ContactsViewModel
    {
        public ContentItem Roastery{ get; set; }

        public List<ContentItem> Cafes { get; set; }
    }
}