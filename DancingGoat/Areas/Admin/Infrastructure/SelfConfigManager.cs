using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using DancingGoat.Areas.Admin.Models;
using DancingGoat.Areas.Admin.Infrastructure;
using System.Configuration;

namespace DancingGoat.Areas.Admin.Helpers
{
    public class SelfConfigManager
    {
        private readonly SubscriptionProvider _subscriptionProvider;
        private readonly ProjectProvider _projectProvider;

        public SelfConfigManager(SubscriptionProvider subscriptionProvider, ProjectProvider projectProvider)
        {
            _subscriptionProvider = subscriptionProvider ?? throw new ArgumentNullException(nameof(subscriptionProvider));
            _projectProvider = projectProvider ?? throw new ArgumentNullException(nameof(projectProvider));
        }

        public void SetSharedProjectIdAsync()
        {
            SetProjectIdAndExpirationAsync(AppSettingProvider.DefaultProjectId.Value);
        }

        public async Task<Guid> DeployAndSetSampleProject(string token, UserModel user, IEnumerable<SubscriptionModel> subscriptions = null)
        {
            var administeredSubscriptionId = user.AdministeredSubscriptions.FirstOrDefault()?.SubscriptionId;
            var actualSubscriptions = subscriptions ?? await _subscriptionProvider.GetSubscriptionsAsync(token);
            var administeredSubscription = actualSubscriptions.FirstOrDefault(s => s.SubscriptionId == administeredSubscriptionId);

            if (administeredSubscriptionId.HasValue && administeredSubscription != null)
            {
                var project = await _projectProvider.DeploySampleAsync(token, administeredSubscriptionId.Value);
                SetProjectIdAndExpirationAsync(project.ProjectId.Value, administeredSubscription.CurrentPlan?.EndAt);
                await _projectProvider.RenameProjectAsync(token, project.ProjectId.Value);

                return project.ProjectId.Value;
            }
            else
            {
                throw new Exception("There was no subscription found to deploy the sample project to.");
            }
        }

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