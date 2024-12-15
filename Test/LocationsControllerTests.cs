using Lab6.Controllers;
using Lab6.Data;
using Lab6.Models;
using Lab6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lab6.Tests
{
    public class LocationsControllerTests
    {
        private readonly DbContextOptions<AdvertisingDbContext> _options;

        public LocationsControllerTests()
        {
            // ”никальное им€ базы данных дл€ каждого теста
            _options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task PostLocation_ReturnsCreatedLocation()
        {
            // Arrange
            using var context = new AdvertisingDbContext(_options);
            var locationViewModel = new LocationViewModel { Name = "NewLocation", LocationDescription = "NewDescription", AdTypeId = 1, AdDescription = "NewAdDescription", Cost = 300 };
            var controller = new LocationsController(context);

            // Act
            var result = await controller.PostLocation(locationViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.NotNull(createdResult);
            Assert.Equal("GetLocation", createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]); // »спользуйте 1, так как это будет первый добавленный элемент
        }

        [Fact]
        public async Task PutLocation_ReturnsNoContent()
        {
            // Arrange
            using var context = new AdvertisingDbContext(_options);
            var location = new Location { LocationId = 1, Name = "Location1", LocationDescription = "Description1", AdTypeId = 1, AdDescription = "AdDescription1", Cost = 100 };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var locationViewModel = new LocationViewModel { LocationId = 1, Name = "UpdatedLocation", LocationDescription = "UpdatedDescription", AdTypeId = 2, AdDescription = "UpdatedAdDescription", Cost = 400 };
            var controller = new LocationsController(context);

            // Act
            var result = await controller.PutLocation(1, locationViewModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteLocation_ReturnsNoContent()
        {
            // Arrange
            using var context = new AdvertisingDbContext(_options);
            var location = new Location { LocationId = 1, Name = "Location1", LocationDescription = "Description1", AdTypeId = 1, AdDescription = "AdDescription1", Cost = 100 };
            context.Locations.Add(location);
            await context.SaveChangesAsync();

            var controller = new LocationsController(context);

            // Act
            var result = await controller.DeleteLocation(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}