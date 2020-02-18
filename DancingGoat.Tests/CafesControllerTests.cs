using DancingGoat.Controllers;
using DancingGoat.Models;
using DancingGoat.Repositories;
using FakeItEasy;
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
            ICafesRepository fakeRepo = null;

            // Example of Moq
            var moqRepo = new Mock<ICafesRepository>();
            moqRepo.Setup(repo => repo.GetCafes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(GetCafes());
            fakeRepo = moqRepo.Object;

            // Example of FakeItEasy
            fakeRepo = A.Fake<ICafesRepository>();
            A.CallTo(() => fakeRepo.GetCafes(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(GetCafes());

            var controller = new CafesController(fakeRepo);

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
