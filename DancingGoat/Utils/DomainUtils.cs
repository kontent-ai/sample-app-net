using System;

namespace DancingGoat.Utils
{
    public static class DomainUtils
    {
        public static string GetSubdomain(this Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                var splittedResult = url.Host.Split('.');
                if (splittedResult.Length > 2)
                {
                    return splittedResult[0];
                }
            }

            return null;
        }
    }
}