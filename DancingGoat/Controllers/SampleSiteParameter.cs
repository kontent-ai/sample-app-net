using KenticoCloud.Delivery;

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