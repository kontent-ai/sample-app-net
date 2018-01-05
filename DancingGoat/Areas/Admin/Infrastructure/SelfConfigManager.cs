using System;
using System.Configuration;

namespace DancingGoat.Areas.Admin.Helpers
{
    public class SelfConfigManager
    {
        public void SetProjectIdAndExpirationAsync(Guid projectId, DateTime? subscriptionExpiresAt = null)
        {
            try
            {
                AppSettingProvider.ProjectId = projectId;

                if (subscriptionExpiresAt.HasValue)
                {
                    AppSettingProvider.SubscriptionExpiresAt = subscriptionExpiresAt;
                }
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new ConfigurationErrorsException($"There was a problem when setting the \"ProjectId\" environment variable to \"{projectId}\". You can set it up manually in your environment (App service - Application Settings, web.config etc.).", ex);
            }
        }
    }
}