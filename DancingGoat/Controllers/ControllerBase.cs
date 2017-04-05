using DancingGoat.Models;
using KenticoCloud.Delivery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DancingGoat.Controllers
{
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