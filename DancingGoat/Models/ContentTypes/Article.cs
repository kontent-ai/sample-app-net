namespace DancingGoat.Models
{
    public partial class Article : IMetadata, IDetailItem
    {
        public string Type => System.Type;
        public string Id => System.Id;
    }
}