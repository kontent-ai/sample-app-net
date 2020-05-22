namespace DancingGoat.Models
{
    public partial class Accessory : IMetadata, IDetailItem
    {
        public string Type => System.Type;
        public string Id => System.Id;
    }
}