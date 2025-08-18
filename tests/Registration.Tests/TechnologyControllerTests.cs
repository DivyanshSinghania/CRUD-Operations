using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Registration.API.Controllers;
using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using Xunit;

namespace Registration.Tests
{
    public class TechnologyControllerTests
    {
        private readonly Mock<ITechnologiesService> _serviceMock;
        private readonly TechnologiesController _controller;

        public TechnologyControllerTests()
        {
            _serviceMock = new Mock<ITechnologiesService>();
            _controller = new TechnologiesController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTechnologies()
        {
            // Arrange
            var mockData = new List<TechnologiesDto>
            {
                new TechnologiesDto { Id = 1, TechnologyStack = "C#" },
                new TechnologiesDto { Id = 2, TechnologyStack = "Java" }
            };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(mockData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<TechnologiesDto>>(okResult.Value);
            Assert.Equal(2, ((List<TechnologiesDto>)returnValue).Count);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WithTechnology()
        {
            // Arrange
            var techDto = new TechnologiesDto { Id = 1, TechnologyStack = "C#" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(techDto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TechnologiesDto>(okResult.Value);
            Assert.Equal("C#", returnValue.TechnologyStack);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTechnologyDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((TechnologiesDto)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Add_ReturnsCreatedAtActionResult_WithCreatedTechnology()
        {
            // Arrange
            var techDto = new TechnologiesDto { TechnologyStack = "Python" };
            var createdDto = new TechnologiesDto { Id = 1, TechnologyStack = "Python" };
            _serviceMock.Setup(s => s.AddAsync(techDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Add(techDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TechnologiesDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WithUpdatedTechnology()
        {
            // Arrange
            var techDto = new TechnologiesDto { Id = 1, TechnologyStack = "UpdatedTech" };
            _serviceMock.Setup(s => s.UpdateAsync(1, techDto)).ReturnsAsync(techDto);

            // Act
            var result = await _controller.Update(1, techDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TechnologiesDto>(okResult.Value);
            Assert.Equal("UpdatedTech", returnValue.TechnologyStack);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenTechnologyIsDeleted()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTechnologyDoesNotExist()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
