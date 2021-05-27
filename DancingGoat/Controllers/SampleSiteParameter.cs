using Kentico.Kontent.Delivery.Abstractions;

namespace DancingGoat.Controllers
{
    public class SampleSiteParameter : IQueryParameter
    {
        public string GetQueryStringParameter()
        {
            return "samplesite=1";
        }
    }
}