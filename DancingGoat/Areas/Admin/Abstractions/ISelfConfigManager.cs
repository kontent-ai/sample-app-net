using System;

namespace DancingGoat.Areas.Admin.Abstractions
{
    public interface ISelfConfigManager
    {
        void SetProjectIdAndExpirationAsync(Guid projectId, DateTime? subscriptionExpiresAt = null);
    }
}
