using System;

namespace DancingGoat
{
    /// <summary>
    /// Configuration section representing App's default configuration
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// URL of the Kentico Kontent app
        /// </summary>
        public string KenticoKontentUrl { get; set; }

        /// <summary>
        /// Project ID of a reference Kentico Kontent sample project.
        /// </summary>
        public Guid DefaultProjectId { get; set; }

        /// <summary>
        /// Expiration date of a subscription the <see cref="Kentico.Kontent.Delivery.DeliveryOptions.ProjectId"/> is bound to.
        /// </summary>
        public DateTime? SubscriptionExpiresAt { get; set; }
    }
}
