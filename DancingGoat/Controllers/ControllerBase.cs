using DancingGoat.Areas.Admin;
using DancingGoat.Infrastructure;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
    [SelfConfigActionFilter]
    public class ControllerBase : AsyncController
    {
        protected readonly DeliveryClient client;

        public ControllerBase()
        {
            var projectId = AppSettingProvider.ProjectId ?? AppSettingProvider.DefaultProjectId;
            client = new DeliveryClient(projectId.Value.ToString());
            client.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            client.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
        }
    }
}