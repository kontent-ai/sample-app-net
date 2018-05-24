using System;

using DancingGoat.Models;
using DancingGoat.Utils;

using KenticoCloud.Delivery;

namespace DancingGoat.Helpers
{
    public class DeliveryClientFactory
    {
        public static DeliveryClient CreateDeliveryClient()
        {
            // Use the provider to get environment variables.
            var provider = new ConfigurationManagerProvider();

            // Build DeliveryOptions with default or explicit values.
            var options = provider.GetDeliveryOptions();

            options.ProjectId = GetProjectId();

            var clientInstance = new DeliveryClient(options);
            clientInstance.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            clientInstance.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
            return clientInstance;
        }

        public static string GetProjectId()
        {
            var requestUrl = System.Web.HttpContext.Current.Request.Url;
            var subdomain = requestUrl.GetSubdomain();
            var decodedGuid = GuidUtils.FromShortString(subdomain);

            if (decodedGuid == null)
            {
                throw new ArgumentNullException(nameof(decodedGuid));
            }

            return decodedGuid.ToString();
        }
    }
}