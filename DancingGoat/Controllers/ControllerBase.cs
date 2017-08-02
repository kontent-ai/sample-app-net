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
        protected readonly DeliveryClient client;

        public ControllerBase()
        {
            string projectId = ConfigurationManager.AppSettings["ProjectId"] ?? ConfigurationManager.AppSettings["DefaultProjectId"];
            client = new DeliveryClient(projectId);
            client.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            client.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
        }
    }
}