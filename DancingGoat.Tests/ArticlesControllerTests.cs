using DancingGoat.Controllers;
using DancingGoat.Models;
using Moq;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using RichardSzalay.MockHttp;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Kentico.Kontent.Delivery.Builders.DeliveryClient;

namespace DancingGoat.Tests
{
    public class ArticlesControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAnArticle()
        {
            // Arrange
            var config = new Mock<IConfiguration>();

            MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
            mockHttp.When($"https://deliver.kontent.ai/975bf280-fd91-488c-994c-2f04416e5ee3/items?elements.url_pattern%5Beq%5D=on_roasts&depth=1&language={CultureInfo.CurrentCulture}&system.type=article")
                .Respond("application/json", File.ReadAllText(Path.Combine(Environment.CurrentDirectory, $"on_roasts.json")));
            IDeliveryClient client = DeliveryClientBuilder.WithProjectId("975bf280-fd91-488c-994c-2f04416e5ee3").WithDeliveryHttpClient(new DeliveryHttpClient(mockHttp.ToHttpClient())).WithTypeProvider(new CustomTypeProvider()).Build();
            var factory = new Mock<IDeliveryClientFactory>();
            factory.Setup(m => m.Get()).Returns(client);

            ArticlesController controller = new ArticlesController(config.Object, factory.Object);

            // Act
            var result = await controller.Show("on_roasts");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<Article>(viewResult.ViewData.Model);
            Assert.Equal("On Roasts", viewModel.Title);
        }
    }
}
