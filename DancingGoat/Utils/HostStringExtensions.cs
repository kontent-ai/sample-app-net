using Microsoft.AspNetCore.Http;

namespace DancingGoat.Utils
{
    public static class HostStringExtensions
    {
        public static string GetSubdomain(this HostString host)
            => host.Host.Split('.') switch
            {
                string[] {Length: > 2} split => split[0],
                _ => null
            };
    }
}