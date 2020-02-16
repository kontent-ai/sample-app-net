using DancingGoat.Controllers;
using DancingGoat.Models;
using DancingGoat.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DancingGoat.Tests
{
    public class CafesControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithTwoListsOfCafes()
        {
            // Arrange
            var mockRepo = new Mock<ICafesRepository>();
            mockRepo.Setup(repo => repo.GetCafes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(GetCafes());
            var controller = new CafesController(mockRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var viewModel = Assert.IsType<CafesViewModel>(viewResult.ViewData.Model);
            Assert.Single(viewModel.CompanyCafes);
            Assert.Equal(2, viewModel.PartnerCafes.Count());
        }

        private List<Cafe> GetCafes()
        {
            var cafes = new List<Cafe>
            {
                new Cafe()
                {
                    City = "New York",
                    Country = "USA"
                },
                new Cafe()
                {
                    City = "Brno",
                    Country = "Czechia"
                },
                new Cafe()
                {
                    City = "Prague",
                    Country = "Czechia"
                }
            };
            return cafes;
        }
    }
}
