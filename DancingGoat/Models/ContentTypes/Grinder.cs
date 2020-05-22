namespace DancingGoat.Models
{
    public partial class Grinder : IMetadata, IDetailItem
    {
        public string Type => System.Type;
        public string Id => System.Id;
    }
}