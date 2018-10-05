using DancingGoat.Utils;
using KenticoCloud.ContentManagement.Helpers;
using KenticoCloud.ContentManagement.Helpers.Configuration;

namespace DancingGoat.Helpers
{
    public sealed class EditLinkHelper
    {
        public static EditLinkBuilder GetBuilder() {
            var projectId = ProjectUtils.GetProjectId();
            var linkBuilderOptions = new ContentManagementHelpersOptions() { ProjectId = projectId };
            return new EditLinkBuilder(linkBuilderOptions);
        }
    }
}