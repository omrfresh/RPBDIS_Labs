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
    public class OrderServicesControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }


        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsValid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            context.Orders.Add(new Order { OrderId = 1});
            context.AdditionalServices.Add(new AdditionalService { AdditionalServiceId = 1, Name = "Service1" });
            await context.SaveChangesAsync();

            var controller = new OrderServicesController(context);
            var orderService = new OrderService { OrderId = 1, ServiceId = 1, TotalCost = 100 };

            // Act
            var result = await controller.Create(orderService);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.Equal(1, await context.OrderServices.CountAsync());
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new OrderServicesController(context);
            controller.ModelState.AddModelError("TotalCost", "Required");

            var invalidOrderService = new OrderService();

            // Act
            var result = await controller.Create(invalidOrderService);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidOrderService, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenOrderServiceDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new OrderServicesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WhenOrderServiceExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var orderService = new OrderService { OrderServiceId = 1, OrderId = 1, ServiceId = 1, TotalCost = 100 };
            context.OrderServices.Add(orderService);
            await context.SaveChangesAsync();

            var controller = new OrderServicesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderService>(viewResult.ViewData.Model);
            Assert.Equal(orderService.OrderServiceId, model.OrderServiceId);
        }

    }
}