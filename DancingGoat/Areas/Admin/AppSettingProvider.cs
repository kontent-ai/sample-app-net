using System;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;

namespace DancingGoat.Areas.Admin
{
    internal static class AppSettingProvider
    {
        private const string PROJECT_ID_KEY_NAME = "ProjectId";
        private const string SUBSCRIPTION_EXPIRES_KEY_NAME = "SubscriptionExpiresAt";
        private const string PREVIEW_TOKEN = "PreviewToken";

        private static readonly Configuration _configuration = WebConfigurationManager.OpenWebConfiguration("~");
        private static DateTime? _subscriptionExpiresAt;
        private static Guid? _projectId;
        private static Guid? _sharedProjectId;
        private static string _previewToken;

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
                if (ConfigurationManager.AppSettings.AllKeys.Contains(SUBSCRIPTION_EXPIRES_KEY_NAME))
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
                if (ConfigurationManager.AppSettings.AllKeys.Contains(PROJECT_ID_KEY_NAME))
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

        internal static Guid? DefaultProjectId
        {
            get
            {
                if (_projectId.HasValue)
                {
                    return _sharedProjectId;
                }
                else
                {
                    if (Guid.TryParse(ConfigurationManager.AppSettings["DefaultProjectId"], out Guid projectId))
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

        internal static string PreviewToken
        {
            get
            {
                if (!string.IsNullOrEmpty(_previewToken))
                {
                    return _previewToken;
                }
                else
                {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(PREVIEW_TOKEN))
                    {
                        _previewToken = ConfigurationManager.AppSettings[PREVIEW_TOKEN];

                        return _previewToken;
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