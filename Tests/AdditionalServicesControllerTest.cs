using Lab4.Controllers;
using Lab4.Data;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class AdditionalServicesControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdditionalServicesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithService()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var service = new AdditionalService
            {
                AdditionalServiceId = 1,
                Name = "Service1",
                Description = "Description1",
                Cost = 100
            };
            context.AdditionalServices.Add(service);
            await context.SaveChangesAsync();

            var controller = new AdditionalServicesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdditionalService>(viewResult.ViewData.Model);
            Assert.Equal(service, model);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdditionalServicesController(context);
            controller.ModelState.AddModelError("Name", "Required");

            var invalidService = new AdditionalService();

            // Act
            var result = await controller.Create(invalidService);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdditionalService>(viewResult.ViewData.Model);
            Assert.Equal(invalidService, model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdditionalServicesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WhenServiceExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var service = new AdditionalService
            {
                AdditionalServiceId = 1,
                Name = "Service1",
                Description = "Description1",
                Cost = 100
            };
            context.AdditionalServices.Add(service);
            await context.SaveChangesAsync();

            var controller = new AdditionalServicesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdditionalService>(viewResult.ViewData.Model);
            Assert.Equal(service, model);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenServiceDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdditionalServicesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenServiceExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var service = new AdditionalService
            {
                AdditionalServiceId = 1,
                Name = "Service1",
                Description = "Description1",
                Cost = 100
            };
            context.AdditionalServices.Add(service);
            await context.SaveChangesAsync();

            var controller = new AdditionalServicesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdditionalService>(viewResult.ViewData.Model);
            Assert.Equal(service, model);
        }
    }
}