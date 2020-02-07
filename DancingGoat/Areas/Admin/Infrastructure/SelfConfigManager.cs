using System;
using DancingGoat.Areas.Admin.Abstractions;

namespace DancingGoat.Areas.Admin.Infrastructure
{
    public class SelfConfigManager : ISelfConfigManager
    {
        private readonly IAppSettingProvider _settingProvider;

        public SelfConfigManager(IAppSettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        public void SetProjectIdAndExpirationAsync(Guid projectId, DateTime? subscriptionExpiresAt = null)
        {
            try
            {
                _settingProvider.ProjectId = projectId;

                if (subscriptionExpiresAt.HasValue)
                {
                    _settingProvider.SubscriptionExpiresAt = subscriptionExpiresAt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"There was a problem when setting the \"ProjectId\" environment variable to \"{projectId}\". You can set it up manually in your environment (App service - Application Settings, web.config etc.).", ex);
            }
        }
    }
}
