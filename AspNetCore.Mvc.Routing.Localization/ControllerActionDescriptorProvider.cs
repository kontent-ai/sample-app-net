using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.Mvc.Routing.Localization
{
    internal class ControllerActionDescriptorProvider : IControllerActionDescriptorProvider
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public ControllerActionDescriptorProvider(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public IEnumerable<ControllerActionDescriptor> Get()
        {
            return _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Select(s => s as ControllerActionDescriptor);
        }
    }
}
