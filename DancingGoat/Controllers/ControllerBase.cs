using System;
using System.Globalization;
using System.Web.Mvc;

using DancingGoat.Helpers;
using DancingGoat.Localization;
using DancingGoat.Models;
using DancingGoat.Utils;
using KenticoCloud.Delivery;

namespace DancingGoat.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected readonly IDeliveryClient baseClient = new SampleDeliveryClient();
        protected readonly IDeliveryClient client;

        public ControllerBase()
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;
            if (currentCulture.Equals(LanguageClient.DEFAULT_LANGUAGE, StringComparison.InvariantCultureIgnoreCase))
            {
                client = baseClient;
            }
            else
            {
                client = new LanguageClient(baseClient, currentCulture);
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Errors/Error.cshtml"
            };
        }

        public static DeliveryClient CreateDeliveryClient()
        {
            // Use the provider to get environment variables.
            var provider = new ConfigurationManagerProvider();

            // Build DeliveryOptions with default or explicit values.
            var options = provider.GetDeliveryOptions();

            options.ProjectId = ProjectUtils.GetProjectId();
            var clientInstance = new DeliveryClient(options);
            clientInstance.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            clientInstance.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
            return clientInstance;
        }
    }
}

