using Lab4.Controllers;
using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class EmployeesControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new EmployeesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithEmployee()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Position = "Manager" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var controller = new EmployeesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
            Assert.Equal(employee, model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsValid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new EmployeesController(context);
            var employee = new Employee { FirstName = "John", LastName = "Doe", Position = "Manager" };

            // Act
            var result = await controller.Create(employee);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.Equal(1, await context.Employees.CountAsync());
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new EmployeesController(context);
            controller.ModelState.AddModelError("FirstName", "Required");

            var employee = new Employee();

            // Act
            var result = await controller.Create(employee);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(employee, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new EmployeesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WhenEmployeeExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Position = "Manager" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var controller = new EmployeesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
            Assert.Equal(employee, model);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new EmployeesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenEmployeeExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var employee = new Employee { EmployeeId = 1, FirstName = "John", LastName = "Doe", Position = "Manager" };
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var controller = new EmployeesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Employee>(viewResult.ViewData.Model);
            Assert.Equal(employee, model);
        }
    }
}