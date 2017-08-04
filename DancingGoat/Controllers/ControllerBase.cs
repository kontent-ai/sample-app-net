using System;
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
            Guid? projectId = AppSettingProvider.ProjectId ?? AppSettingProvider.DefaultProjectId;

            client = new DeliveryClient(projectId.Value.ToString())
            {
                CodeFirstModelProvider = {TypeProvider = new CustomTypeProvider()},
                ContentLinkUrlResolver = new CustomContentLinkUrlResolver()
            };
        }
    }
}