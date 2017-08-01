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

        private static readonly Configuration _configuration = WebConfigurationManager.OpenWebConfiguration("~");
        private static DateTime? _subscriptionExpiresAt;
        private static Guid? _projectId;
        private static Guid? _sharedProjectId;

        internal static DateTime? SubscriptionExpiresAt
        {
            get
            {
                if (_subscriptionExpiresAt.HasValue)
                {
                    return _subscriptionExpiresAt;
                }
                else
                {
                    if (DateTime.TryParse(ConfigurationManager.AppSettings[SUBSCRIPTION_EXPIRES_KEY_NAME], out DateTime subscriptionExpiresAt))
                    {
                        _subscriptionExpiresAt = subscriptionExpiresAt;

                        return _subscriptionExpiresAt;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set
            {
                if (DateTime.TryParse(ConfigurationManager.AppSettings[SUBSCRIPTION_EXPIRES_KEY_NAME], out DateTime subscriptionExpiresAt))
                {
                    _configuration.AppSettings.Settings[SUBSCRIPTION_EXPIRES_KEY_NAME].Value = value.Value.ToString("o");
                }
                else
                {
                    _configuration.AppSettings.Settings.Add(SUBSCRIPTION_EXPIRES_KEY_NAME, value.Value.ToString("o"));
                }

                _configuration.Save();
                _subscriptionExpiresAt = value;
            }
        }

        internal static Guid? ProjectId
        {
            get
            {
                if (_projectId.HasValue)
                {
                    return _projectId;
                }
                else
                {
                    if (Guid.TryParse(ConfigurationManager.AppSettings[PROJECT_ID_KEY_NAME], out Guid projectId))
                    {
                        _projectId = projectId;

                        return _projectId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            set
            {
                if (Guid.TryParse(ConfigurationManager.AppSettings[PROJECT_ID_KEY_NAME], out Guid projectId))
                {
                    _configuration.AppSettings.Settings[PROJECT_ID_KEY_NAME].Value = value.ToString();
                }
                else
                {
                    _configuration.AppSettings.Settings.Add(PROJECT_ID_KEY_NAME, value.ToString());
                }

                _configuration.Save();
                _projectId = value;
            }
        }

        internal static Guid? SharedProjectId
        {
            get
            {
                if (_projectId.HasValue)
                {
                    return _sharedProjectId;
                }
                else
                {
                    if (Guid.TryParse(ConfigurationManager.AppSettings["SharedProjectId"], out Guid projectId))
                    {
                        _sharedProjectId = projectId;

                        return _sharedProjectId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}