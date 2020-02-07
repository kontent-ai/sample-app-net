using System;
using System.Linq;
using DancingGoat.Models;
using Microsoft.Extensions.Options;
using DancingGoat.Areas.Admin.Abstractions;
using Microsoft.Extensions.Configuration;
using Kentico.Kontent.Delivery;

namespace DancingGoat.Areas.Admin
{
    public class AppSettingProvider : IAppSettingProvider
    {
        private readonly AppConfiguration _settings;
        private readonly IConfiguration _configuration;


        private DateTime? _subscriptionExpiresAt;
        private Guid? _projectId;

        // Using IOptionsSnapshot to get the reloaded configuration on change
        public AppSettingProvider(IOptionsSnapshot<AppConfiguration> options, IConfiguration configuration)
        {
            _settings = options.Value;
            _configuration = configuration;
        }

        public DateTime? SubscriptionExpiresAt
        {
            get
            {
                return _settings.SubscriptionExpiresAt;
            }
            set
            {

                // TODO: Set configuration on the fly in asp.net core 2.x???

                if (value.HasValue)
                {
                    _settings.SubscriptionExpiresAt = value.Value;
                }

                // Creating new settings cannot be done through the indexer, hence .Add().
                /*if (ConfigurationManager.AppSettings.AllKeys.Contains(SUBSCRIPTION_EXPIRES_KEY_NAME))
            {
                _configuration.AppSettings.Settings[SUBSCRIPTION_EXPIRES_KEY_NAME].Value = value.Value.ToString("o");
            }
            else
            {
                _configuration.AppSettings.Settings.Add(SUBSCRIPTION_EXPIRES_KEY_NAME, value.Value.ToString("o"));
            }
            // Do we want to save to the appsettings.json those changes??
            //_configuration.Save();*/
                _subscriptionExpiresAt = value;
            }
        }

        public Guid? ProjectId
        {
            get
            {
                if (_projectId.HasValue)
                {
                    return _projectId;
                }
                else
                {
                    if (Guid.TryParse(_configuration.GetSection(nameof(DeliveryOptions)).GetValue<string>("ProjectId"), out var projectId))
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
                // TODO: same as the _subscriptionExpiresAt
                if (value.HasValue)
                {
                    _configuration["DeliveryOptions:ProjectId"] = value.Value.ToString();
                }

                _configuration["DeliveryOptions:ProjectId"] = value.Value.ToString();
                _projectId = value;
            }
        }

        public Guid? DefaultProjectId
        {
            get
            {
                Guid.TryParse(_configuration["AppConfiguration:DefaultProjectId"], out var projectId);
                return projectId;
            }
        }

        public string KenticoKontentUrl => _configuration["AppConfiguration:KenticoKontentUrl"];

    }
}