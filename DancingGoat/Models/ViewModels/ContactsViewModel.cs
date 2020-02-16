using System.Collections.Generic;

namespace DancingGoat.Models
{
    public class ContactsViewModel
    {
        public Cafe Roastery { get; set; }

        public IEnumerable<Cafe> Cafes { get; set; }
    }
}