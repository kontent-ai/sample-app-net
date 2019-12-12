using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat.Areas.Admin.Abstractions
{
    public interface ISelfConfigManager
    {
        void SetProjectIdAndExpirationAsync(Guid projectId, DateTime? subscriptionExpiresAt = null);
    }
}
