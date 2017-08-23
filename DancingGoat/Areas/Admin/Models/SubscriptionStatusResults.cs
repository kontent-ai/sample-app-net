using System;
using System.Collections.Generic;

namespace DancingGoat.Areas.Admin.Models
{
    public class SubscriptionStatusResults
    {
        public DateTime? EndAt { get; set; }
        public SubscriptionStatus Status { get; set; }
        public IEnumerable<ProjectModel> Projects { get; set; }
    }

    public enum SubscriptionStatus
    {
        Active,
        Inactive,
        Expired
    }
}