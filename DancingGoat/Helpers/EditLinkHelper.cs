

using KenticoCloud.ContentManagement.Helpers;
using KenticoCloud.ContentManagement.Helpers.Configuration;
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
            var projectId = configuration.GetSection("AppConfiguration")["ProjectId"].ToString() ?? configuration.GetSection("AppConfiguration")["DefaultProjectId"].ToString();
            var linkBuilderOptions = new ContentManagementHelpersOptions() { ProjectId = projectId };
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