using AspNetCore.Mvc.Routing.Localization.Attributes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCore.Mvc.Routing.Localization.Tests
{
    public class LocalizedRoutingProviderTests
    {
        private IControllerActionDescriptorProvider _controllerActionDescriptorProvider;
        private LocalizedRouteProvider _localizedRoutingProvider;

        public LocalizedRoutingProviderTests()
        {
            _controllerActionDescriptorProvider = Substitute.For<IControllerActionDescriptorProvider>();
            _localizedRoutingProvider = new LocalizedRouteProvider(
                _controllerActionDescriptorProvider,
                Options.Create(new Microsoft.AspNetCore.Builder.RequestLocalizationOptions
                {
                    SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") }
                })
            );
        }

        [Fact]
        public async Task ProvideRouteAsync_NotAnyRoute_GetNull()
        {
            var route = await _localizedRoutingProvider.ProvideRouteAsync("en-US", "controller", "action", LocalizationDirection.TranslatedToOriginal);

            route.Should().BeNull();
            _controllerActionDescriptorProvider.Received(1).Get();
        }

        [Fact]
        public async Task ProvideRouteAsync_NotAnyRouteAndCallMoreThenOne_JustOneInit()
        {
            await _localizedRoutingProvider.ProvideRouteAsync("en-US", "controller", "action", LocalizationDirection.TranslatedToOriginal);
            await _localizedRoutingProvider.ProvideRouteAsync("en-US", "controller", "action", LocalizationDirection.TranslatedToOriginal);

            _controllerActionDescriptorProvider.Received(1).Get();
        }

        [Theory]
        [InlineData(typeof(NoAttributeController), "Index", "NoAttributeController", "Index")]
        [InlineData(typeof(HomeController), "Index", "TranslatedHome", "TranslatedIndex")]
        [InlineData(typeof(HomeController1), "Index", "TranslatedHome", "TranslatedIndex")]
        [InlineData(typeof(HomeController2), "Index", "TranslatedHome", "Index")]
        [InlineData(typeof(HomeController3), "Index", "TranslatedHome", "TranslatedIndex")]
        [InlineData(typeof(HomeController4), "Index", "TranslatedHome", "Index")]
        public async Task ProvideRouteAsync_AttributesCombinations_GetsLocalizedRoute(Type originalController, string originalAction,
            string controller, string action)
        {
            _controllerActionDescriptorProvider.Get()
                .Returns(GetControllerActionDescriptor(originalController, originalAction));

            var route = await _localizedRoutingProvider.ProvideRouteAsync("en-US", originalController.Name, "Index", LocalizationDirection.OriginalToTranslated);

            route.Action.Should().Be(action);
            route.Controller.Should().Be(controller);
        }

        [Theory]
        [InlineData(typeof(NoAttributeController), "Index", "NoAttributeController", "Index")]
        [InlineData(typeof(HomeController), "Index", "TranslatedHome", "TranslatedIndex")]
        [InlineData(typeof(HomeController1), "Index", "TranslatedHome", "TranslatedIndex")]
        [InlineData(typeof(HomeController2), "Index", "TranslatedHome", "Index")]
        [InlineData(typeof(HomeController3), "Index", "TranslatedHome", "TranslatedIndex")]
        [InlineData(typeof(HomeController4), "Index", "TranslatedHome", "Index")]
        public async Task ProvideRouteAsync_AttributesCombinations_GetsOriginalRoute(Type originalController, string originalAction,
            string controller, string action)
        {
            _controllerActionDescriptorProvider.Get()
                .Returns(GetControllerActionDescriptor(originalController, originalAction));

            var route = await _localizedRoutingProvider.ProvideRouteAsync("en-US", controller, action, LocalizationDirection.TranslatedToOriginal);

            route.Action.Should().Be("Index");
            route.Controller.Should().Be(originalController.Name);
        }

        private static List<ControllerActionDescriptor> GetControllerActionDescriptor(Type originalController, string originalAction)
        {
            return new List<ControllerActionDescriptor>
                {
                    new ControllerActionDescriptor
                    {
                        RouteValues = new Dictionary<string, string>
                        {
                            { "controller", originalController.Name},
                            { "action", originalAction}
                        },
                        ControllerTypeInfo = originalController.GetTypeInfo(),
                        MethodInfo = originalController.GetMethod(originalAction)
                    }
                };
        }

        private sealed class NoAttributeController
        {
            public ActionResult Index()
            {
                return null;
            }
        }

        [LocalizedRoute("en-US", "TranslatedHome")]
        private sealed class HomeController
        {
            [LocalizedRoute("en-US", "TranslatedIndex")]
            public ActionResult Index()
            {
                return null;
            }
        }

        [LocalizedRoute("en-US", "TranslatedHome")]
        private sealed class HomeController1
        {
            [Route("TranslatedIndex")]
            public ActionResult Index()
            {
                return null;
            }
        }

        [LocalizedRoute("en-US", "TranslatedHome")]
        private sealed class HomeController2
        {
            public ActionResult Index()
            {
                return null;
            }
        }

        [Route("TranslatedHome")]
        private sealed class HomeController3
        {
            [Route("TranslatedIndex")]
            public ActionResult Index()
            {
                return null;
            }
        }

        [Route("TranslatedHome")]
        private sealed class HomeController4
        {
            public ActionResult Index()
            {
                return null;
            }
        }
    }
}
