using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
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
                Nullable<DateTime> @null = null;

                return subscriptionExpiresAtParsed ? subscriptionExpiresAt : @null;
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

                return parsed ? new Nullable<Guid>(projectId) : null;
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