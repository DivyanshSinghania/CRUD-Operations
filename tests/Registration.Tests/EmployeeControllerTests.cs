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
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockService;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockService = new Mock<IEmployeeService>();
            _controller = new EmployeeController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithList()
        {
            // Arrange
            var employees = new List<EmployeeDto>
            {
                new EmployeeDto { Employee_Id = 1, Name = "John", Email = "john@example.com", Department = "IT", Designation = "Dev" }
            };
            _mockService.Setup(s => s.GetAllEmployeesAsync()).ReturnsAsync(employees);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnEmployees = Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(okResult.Value);
            Assert.Single(returnEmployees);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenFound()
        {
            var emp = new EmployeeDto { Employee_Id = 1, Name = "John" };
            _mockService.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync(emp);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnEmp = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal(1, returnEmp.Employee_Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((EmployeeDto?)null);

            var result = await _controller.GetById(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction()
        {
            var emp = new EmployeeDto { Employee_Id = 1, Name = "John" };
            _mockService.Setup(s => s.CreateEmployeeAsync(emp)).ReturnsAsync(emp);

            var result = await _controller.Create(emp);

            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdAt.ActionName);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccess()
        {
            var emp = new EmployeeDto { Employee_Id = 1, Name = "John" };
            _mockService.Setup(s => s.UpdateEmployeeAsync(1, emp)).ReturnsAsync(true);

            var result = await _controller.Update(1, emp);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            var emp = new EmployeeDto { Employee_Id = 2, Name = "John" };

            var result = await _controller.Update(1, emp);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ID mismatch", badRequest.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccess()
        {
            _mockService.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(false);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
