using KenticoCloud.Delivery;
using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class ContactsViewModel
    {
        public ContentItem Roastery { get; set; }

        public IReadOnlyList<ContentItem> Cafes { get; set; }
    }
}