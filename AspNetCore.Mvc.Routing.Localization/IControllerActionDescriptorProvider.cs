using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;

namespace AspNetCore.Mvc.Routing.Localization
{
    public interface IControllerActionDescriptorProvider
    {
        IEnumerable<ControllerActionDescriptor> Get();
    }
}