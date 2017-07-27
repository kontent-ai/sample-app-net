using System;
using System.Configuration;
using System.Web.Configuration;

namespace DancingGoat.Areas.Admin
{
    internal static class AppSettingProvider
    {
        private const string PROJECT_ID_KEY_NAME = "ProjectId";
        private const string SUBSCRIPTION_EXPIRES_KEY_NAME = "SubscriptionExpiresAt";

        // TODO Consider thread safety.
        private static Configuration _configuration = WebConfigurationManager.OpenWebConfiguration("~");

        internal static DateTime? SubscriptionExpiresAt
        {
            get
            {
                DateTime subscriptionExpiresAt;
                bool subscriptionExpiresAtParsed = DateTime.TryParse(ConfigurationManager.AppSettings[SUBSCRIPTION_EXPIRES_KEY_NAME], out subscriptionExpiresAt);

                return subscriptionExpiresAtParsed ? subscriptionExpiresAt : (DateTime?)null;
            }
            set
            {
                try
                {
                    _configuration.AppSettings.Settings[SUBSCRIPTION_EXPIRES_KEY_NAME].Value = value.Value.ToString("o");
                    _configuration.Save();
                }
                catch (Exception ex)
                {
                    // UNDONE
                }

            }
        }

        internal static Guid? ProjectId
        {
            get
            {
                Guid projectId;
                bool parsed = Guid.TryParse(ConfigurationManager.AppSettings[PROJECT_ID_KEY_NAME], out projectId);

                return parsed ? new Guid?(projectId) : null;
            }
            set
            {
                try
                {
                    _configuration.AppSettings.Settings[PROJECT_ID_KEY_NAME].Value = value.ToString();
                    _configuration.Save();
                }
                catch (Exception ex)
                {
                    // UNDONE
                }
            }
        }
    }
}