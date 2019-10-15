using System;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;

namespace DancingGoat.Areas.Admin
{
    public static class AppSettingProvider
    {
        private const string PROJECT_ID_KEY_NAME = "ProjectId";
        private const string SUBSCRIPTION_EXPIRES_KEY_NAME = "SubscriptionExpiresAt";
        private const string PREVIEW_API_KEY = "PreviewApiKey";
        private const string RESPONSIVE_WIDTHS = "ResponsiveWidths";

        private static readonly Configuration _configuration = WebConfigurationManager.OpenWebConfiguration("~");
        private static DateTime? _subscriptionExpiresAt;
        private static Guid? _projectId;
        private static Guid? _defaultProjectId;
        private static string _previewApiKey;
        private static string _kenticoKontentUrl;

        public static DateTime? SubscriptionExpiresAt
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
                // Creating new settings cannot be done through the indexer, hence .Add().
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

        public static Guid? ProjectId
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
                // Creating new settings cannot be done through the indexer, hence .Add().
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

        public static Guid? DefaultProjectId
        {
            get
            {
                if (_defaultProjectId.HasValue)
                {
                    return _defaultProjectId;
                }
                else
                {
                    if (Guid.TryParse(ConfigurationManager.AppSettings["DefaultProjectId"], out Guid projectId))
                    {
                        _defaultProjectId = projectId;

                        return _defaultProjectId;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static string PreviewApiKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_previewApiKey))
                {
                    return _previewApiKey;
                }
                else
                {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(PREVIEW_API_KEY))
                    {
                        _previewApiKey = ConfigurationManager.AppSettings[PREVIEW_API_KEY];

                        return _previewApiKey;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static string KenticoKontentUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_kenticoKontentUrl))
                {
                    return _kenticoKontentUrl;
                }
                else
                {
                    string url = null;

                    try
                    {
                        url = ConfigurationManager.AppSettings["KenticoKontentUrl"];
                    }
                    catch
                    {
                        // Return the default value below.
                    }

                    if (!string.IsNullOrEmpty(url))
                    {
                        _kenticoKontentUrl = url;

                        return _kenticoKontentUrl;
                    }
                    else
                    {
                        return @"https://app.kontent.ai";
                    }
                }
            }
        }

        public static string[] ResponsiveWidths { get; } =
            ConfigurationManager.AppSettings[RESPONSIVE_WIDTHS]?.Split(',') ?? new string[] { };

        public static bool ResponsiveImagesEnabled { get; } = ResponsiveWidths.Any();
    }
}