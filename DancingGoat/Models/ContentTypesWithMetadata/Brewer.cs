namespace DancingGoat.Models
{
    public partial class Brewer: IMetadata, IDetailItem
    {
        public string Type => System.Type;
        public string Id => System.Id;
    }
}