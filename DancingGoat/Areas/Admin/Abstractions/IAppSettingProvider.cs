using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat.Areas.Admin.Abstractions
{
    public interface IAppSettingProvider
    {
        Guid? GetProjectId();
        string GetKenticoCloudUrl();
        Guid? GetDefaultProjectId();
        void SetProjectId(Guid projectId);
        void SetSubscriptionExpiresAt(DateTime? subscriptionExpiresAt);
    }
}
