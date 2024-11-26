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
    public class LocationsControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }


        [Fact]
        public async Task Details_ReturnsNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new LocationsController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithLocation()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var location = new Location { LocationId = 1, Name = "Location1", LocationDescription = "Description1", Cost = 100 };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var controller = new LocationsController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Location>(viewResult.ViewData.Model);
            Assert.Equal(location, model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsValid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new LocationsController(context);
            var location = new Location { Name = "Location1", LocationDescription = "Description1", Cost = 100, AdTypeId = 1 };

            // Act
            var result = await controller.Create(location);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.Equal(1, await context.Locations.CountAsync());
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new LocationsController(context);
            controller.ModelState.AddModelError("Name", "Required");

            var invalidLocation = new Location();

            // Act
            var result = await controller.Create(invalidLocation);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidLocation, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new LocationsController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WhenLocationExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var location = new Location { LocationId = 1, Name = "Location1", LocationDescription = "Description1", Cost = 100 };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var controller = new LocationsController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Location>(viewResult.ViewData.Model);
            Assert.Equal(location, model);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenLocationDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new LocationsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenLocationExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var location = new Location { LocationId = 1, Name = "Location1", LocationDescription = "Description1", Cost = 100 };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var controller = new LocationsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Location>(viewResult.ViewData.Model);
            Assert.Equal(location, model);
        }
    }
}