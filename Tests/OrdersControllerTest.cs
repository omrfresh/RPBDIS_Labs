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
    public class OrdersControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }


        [Fact]
        public async Task Details_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new OrdersController(context);
            controller.ModelState.AddModelError("TotalCost", "Required");

            var invalidViewModel = new OrderViewModel();

            // Act
            var result = await controller.Create(invalidViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidViewModel, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Delete_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}