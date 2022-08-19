using AspNetCore.Mvc.Routing.Localization.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Xunit;

namespace AspNetCore.Mvc.Routing.Localization.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        private IServiceCollection _serviceCollection;
        private readonly ReadOnlyDictionary<Type, Type> _expectedServices;

        public ServiceCollectionExtensionsTests()
        {
            _serviceCollection = new ServiceCollection();
            _expectedServices = new ReadOnlyDictionary<Type, Type>(
             new Dictionary<Type, Type>
             {
                { typeof(IActionContextAccessor), typeof(ActionContextAccessor) },
                { typeof(ILocalizedRoutingDynamicRouteValueResolver), typeof(LocalizedRoutingDynamicRouteValueResolver) },
                { typeof(ILocalizedRoutingProvider), typeof(LocalizedRouteProvider) },
                { typeof(IControllerActionDescriptorProvider), typeof(ControllerActionDescriptorProvider) },
             }
            );
        }

        [Fact]
        public void AddLocalizedRouting_AllServicesAreRegistered()
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US")
            };

            var serviceProvider = _serviceCollection
                .AddLocalizedRouting(supportedCultures)
                .AddSingleton<IActionDescriptorCollectionProvider, FakeActionDescriptorCollectionProvider>()
                .BuildServiceProvider();

            AssertServiceCollection(serviceProvider, _expectedServices);
        }

        private void AssertServiceCollection(ServiceProvider provider, IDictionary<Type, Type> expectedTypes)
        {
            foreach (var type in expectedTypes)
            {
                var imp = provider.GetRequiredService(type.Key);
                Assert.IsType(type.Value, imp);
            }
        }

        private class FakeActionDescriptorCollectionProvider : IActionDescriptorCollectionProvider
        {
            public ActionDescriptorCollection ActionDescriptors => throw new NotImplementedException();
        }
    }
}
