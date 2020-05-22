namespace DancingGoat.Models
{
    public partial class Home : IMetadata, IDetailItem
    {
        public string Type => System.Type;
        public string Id => System.Id;
    }
}