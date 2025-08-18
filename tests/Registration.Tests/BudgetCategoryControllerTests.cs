using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Registration.API.Controllers;
using Registration.Application.DTOs;
using Registration.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Registration.Tests
{
    public class BudgetCategoryControllerTests
    {
        private readonly Mock<IBudgetCategoryService> _service = new();
        private readonly BudgetCategoryController _controller;

        public BudgetCategoryControllerTests()
        {
            _controller = new BudgetCategoryController(_service.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithList()
        {
            var items = new List<BudgetCategoryDto>
            {
                new() { Id = 1, CategoryType = "Ops", BudgetAmount = 100, FinancialYear = 2025 }
            };
            _service.Setup(s => s.GetAllAsync()).ReturnsAsync(items);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BudgetCategoryDto>>(ok.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var dto = new BudgetCategoryDto { Id = 1, CategoryType = "Ops", BudgetAmount = 100, FinancialYear = 2025 };
            _service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<BudgetCategoryDto>(ok.Value);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((BudgetCategoryDto)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var dto = new BudgetCategoryDto { Id = 10, CategoryType = "Ops", BudgetAmount = 100, FinancialYear = 2025 };
            _service.Setup(s => s.CreateAsync(dto)).ReturnsAsync(dto);

            var result = await _controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", created.ActionName);
            var model = Assert.IsType<BudgetCategoryDto>(created.Value);
            Assert.Equal(10, model.Id);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenUpdated()
        {
            var dto = new BudgetCategoryDto { Id = 1, CategoryType = "Ops2", BudgetAmount = 200, FinancialYear = 2026 };
            _service.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(dto);

            var result = await _controller.Update(1, dto);

            var ok = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<BudgetCategoryDto>(ok.Value);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenMissing()
        {
            var dto = new BudgetCategoryDto { Id = 99, CategoryType = "X", BudgetAmount = 1, FinancialYear = 2025 };
            _service.Setup(s => s.UpdateAsync(99, dto)).ReturnsAsync((BudgetCategoryDto)null);

            var result = await _controller.Update(99, dto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            _service.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            _service.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}