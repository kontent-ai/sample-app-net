using Kentico.Kontent.Delivery;
using Kentico.Kontent.Management.Helpers;
using Kentico.Kontent.Management.Helpers.Configuration;
using Microsoft.Extensions.Configuration;

namespace DancingGoat.Helpers
{
    public sealed class EditLinkHelper
    {
        private static EditLinkHelper _instance = null;
        private static readonly object padlock = new object();
        private readonly IConfiguration configuration;

        public EditLinkBuilder Builder { get; private set; }


        private EditLinkHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
            var projectId = configuration.GetSection(nameof(DeliveryOptions))[nameof(DeliveryOptions.ProjectId)].ToString() ?? configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>().DefaultProjectId.ToString();
            var linkBuilderOptions = new ManagementHelpersOptions() { ProjectId = projectId };
            Builder = new EditLinkBuilder(linkBuilderOptions);
        }

        public static EditLinkHelper GetInstance(IConfiguration configuration)
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new EditLinkHelper(configuration);
                    }
                }
            }
            return _instance;
        }
    }
}