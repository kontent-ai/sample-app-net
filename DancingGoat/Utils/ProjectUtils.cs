using System;

namespace DancingGoat.Utils
{
    public class ProjectUtils
    {
        public static string GetProjectId()
        {
            var requestUrl = System.Web.HttpContext.Current.Request.Url;
            var subdomain = requestUrl.GetSubdomain();
            var decodedGuid = Guid.ParseExact(subdomain, "N");

            if (decodedGuid == null)
            {
                throw new InvalidOperationException(nameof(decodedGuid));
            }

            return decodedGuid.ToString();
        }
    }
}