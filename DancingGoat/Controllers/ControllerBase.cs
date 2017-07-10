using System;
using DancingGoat.Models;
using KenticoCloud.Delivery;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using DancingGoat.Infrastructure;
using DancingGoat.InlineContentItemResolver;

namespace DancingGoat.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected static readonly DeliveryClient baseClient = CreateDeliveryClient();
        public readonly LanguageClient client;

        public ControllerBase()
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            client = new LanguageClient(baseClient, currentCulture);
        }
       

        public static DeliveryClient CreateDeliveryClient()
        {
            var previewToken = ConfigurationManager.AppSettings["PreviewToken"];
            var projectId = ConfigurationManager.AppSettings["ProjectId"];

            var clientInstance = 
                !string.IsNullOrEmpty(previewToken) ? 
                    new DeliveryClient(projectId, previewToken) : 
                    new DeliveryClient(projectId);

            clientInstance.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            clientInstance.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
            clientInstance.InlineContentItemsProcessor.RegisterTypeResolver(new HostedVideoResolver());
            clientInstance.InlineContentItemsProcessor.RegisterTypeResolver(new TweetResolver());
            return clientInstance;
        }
    }
}

