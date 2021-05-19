using Microsoft.AspNetCore.Http;
using System;

namespace DancingGoat.Utils
{
    public static class DomainUtils
    {
        public static string GetSubdomain(this HostString host)
        {
            var splittedResult = host.Host.Split('.');
            if (splittedResult.Length > 2)
            {
                return splittedResult[0];
            }
            return null;
        }
    }
}