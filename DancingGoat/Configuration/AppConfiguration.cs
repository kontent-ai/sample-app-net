using Kontent.Ai.Delivery.Abstractions;
using System;

namespace DancingGoat
{
    /// <summary>
    /// Configuration section representing App's default configuration
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// URL of the Kontent.ai app
        /// </summary>
        public string KontentUrl { get; set; }

        /// <summary>
        /// Delivery options referencing the default Kontent.ai sample project.
        /// </summary>
        public DeliveryOptions DefaultDeliveryOptions { get; set; }

        /// <summary>
        /// Expiration date of a subscription the <see cref="Kontent.Ai.Delivery.Abstractions.DeliveryOptions.ProjectId"/> is bound to.
        /// </summary>
        public DateTime? SubscriptionExpiresAt { get; set; }
    }
}
