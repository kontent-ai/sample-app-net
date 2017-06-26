using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Configuration;
using System.Web.Mvc;
using DancingGoat.InlineContentItemResolver;

namespace DancingGoat.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected readonly DeliveryClient client = new DeliveryClient(ConfigurationManager.AppSettings["ProjectId"]);

        public ControllerBase()
        {
            client.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            client.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
            client.InlineContentItemsProcessor.RegisterTypeResolver(new HostedVideoResolver());
            client.InlineContentItemsProcessor.RegisterTypeResolver(new TweetResolver());
        }
    }
}