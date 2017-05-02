using Microsoft.Owin;
using Owin;
using System.Net;

[assembly: OwinStartupAttribute(typeof(DancingGoat.Startup))]

namespace DancingGoat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // .NET Framework 4.6.1 and lower does not support TLS 1.2 as the default protocol, but Delivery API requires it.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}