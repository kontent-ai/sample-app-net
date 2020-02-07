using System;

namespace DancingGoat.Areas.Admin.Abstractions
{
    public interface IAppSettingProvider
    {
        Guid? ProjectId { get; set; }
        string KenticoKontentUrl { get; }
        Guid? DefaultProjectId { get; }
        DateTime? SubscriptionExpiresAt { get; set; }
    }
}
