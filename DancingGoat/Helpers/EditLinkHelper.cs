using DancingGoat.Areas.Admin;

using KenticoCloud.ContentManagement.Helpers;
using KenticoCloud.ContentManagement.Helpers.Configuration;

namespace DancingGoat.Helpers
{
    public sealed class EditLinkHelper
    {
        private static EditLinkHelper _instance = null;
        private static readonly object padlock = new object();
        public EditLinkBuilder Builder { get; private set; }

        private EditLinkHelper()
        {
            var projectId = AppSettingProvider.ProjectId.ToString() ?? AppSettingProvider.DefaultProjectId.ToString();
            var linkBuilderOptions = new ContentManagementHelpersOptions() { ProjectId = projectId };
            Builder = new EditLinkBuilder(linkBuilderOptions);
        }

        public static EditLinkHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EditLinkHelper();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}