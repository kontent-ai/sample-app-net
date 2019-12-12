using Kentico.Kontent.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DancingGoat.Localization
{
    public interface IDeliveryClientFactory
    {
        IDeliveryClient GetDeliveryClient();
    }
}
