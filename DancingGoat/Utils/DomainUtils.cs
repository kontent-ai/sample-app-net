using System;

namespace DancingGoat.Utils
{
    public static class DomainUtils
    {
        public static string GetSubdomain(this Uri url)
        {
            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;

                if (host.Split('.').Length > 2)
                {
                    int lastIndex = host.LastIndexOf(".");
                    int index = host.LastIndexOf(".", lastIndex - 1);

                    return host.Substring(0, index);
                }
            }

            return null;
        }
    }
}