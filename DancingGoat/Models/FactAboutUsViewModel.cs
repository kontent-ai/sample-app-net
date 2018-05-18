using KenticoCloud.ContentManagement.Helpers.Models;

namespace DancingGoat.Models
{
    public class FactAboutUsViewModel
    {
        public FactAboutUs Fact { get; set; }
        public bool Odd { get; set; }
        public ElementIdentifier ParentItemElementIdentifier { get; set; }
    }
}