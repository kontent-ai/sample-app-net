using System;

namespace DancingGoat.Areas.Admin.Abstractions
{
    public interface IAppSettingProvider
    {
        Guid? GetProjectId();
        string GetKenticoKontentUrl();
        Guid? GetDefaultProjectId();
        void SetProjectId(Guid projectId);
        void SetSubscriptionExpiresAt(DateTime? subscriptionExpiresAt);
    }
}
