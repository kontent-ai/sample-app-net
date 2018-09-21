using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using DancingGoat.Helpers;
using DancingGoat.Localization;
using DancingGoat.Models;
using DancingGoat.Utils;

using KenticoCloud.Delivery;

namespace DancingGoat.Controllers
{
    public class ControllerBase : AsyncController
    {
        protected IDeliveryClient baseClient { get; private set; }
        protected IDeliveryClient client { get; private set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            baseClient = new SampleDeliveryClient(requestContext.HttpContext.ApplicationInstance.Context);

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

        public static DeliveryClient CreateDeliveryClient(HttpContext httpContext)
        {
            // Use the provider to get environment variables.
            var provider = new ConfigurationManagerProvider();

            // Build DeliveryOptions with default or explicit values.
            var options = provider.GetDeliveryOptions();

            options.ProjectId = ProjectUtils.GetProjectId();

            if (ProjectUtils.IsInPreviewMode(httpContext) && !string.IsNullOrEmpty(ProjectUtils.GetPreviewApiKey(httpContext)))
            {
                options.UsePreviewApi = true;
                options.PreviewApiKey = ProjectUtils.GetPreviewApiKey(httpContext);
            }

            var clientInstance = new DeliveryClient(options);
            clientInstance.CodeFirstModelProvider.TypeProvider = new CustomTypeProvider();
            clientInstance.ContentLinkUrlResolver = new CustomContentLinkUrlResolver();
            return clientInstance;
        }
    }
}

