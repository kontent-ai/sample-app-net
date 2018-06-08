using System;

namespace DancingGoat.Utils
{
    public class ProjectUtils
    {
        public static string GetProjectId()
        {
            var requestUrl = System.Web.HttpContext.Current.Request.Url;
            var subdomain = requestUrl.GetSubdomain();

            if (!Guid.TryParseExact(subdomain, "N", out Guid decodedGuid))
            {
                throw new InvalidOperationException(nameof(decodedGuid));
            }

            return decodedGuid.ToString();
        }
    }
}