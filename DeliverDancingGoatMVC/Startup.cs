using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DeliverDancingGoatMVC.Startup))]
namespace DeliverDancingGoatMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
