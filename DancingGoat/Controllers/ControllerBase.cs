using DancingGoat.Infrastructure;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Configuration;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [SelfConfigActionFilter]
    public class ControllerBase : AsyncController
    {
        protected readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        public ControllerBase()
        {
            client.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            client.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
        }
    }
}