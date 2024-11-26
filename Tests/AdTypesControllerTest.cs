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
    public class AdTypesControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenAdTypeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdTypesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithAdType()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var adType = new AdType { AdTypeId = 1, Name = "Type1", Description = "Description1" };
            context.AdTypes.Add(adType);
            await context.SaveChangesAsync();

            var controller = new AdTypesController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdType>(viewResult.ViewData.Model);
            Assert.Equal(adType, model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsValid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdTypesController(context);
            var adType = new AdType { Name = "NewType", Description = "NewDescription" };

            // Act
            var result = await controller.Create(adType);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.Equal(1, await context.AdTypes.CountAsync());
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdTypesController(context);
            controller.ModelState.AddModelError("Name", "Required");

            var adType = new AdType();

            // Act
            var result = await controller.Create(adType);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(adType, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenAdTypeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdTypesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WhenAdTypeExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var adType = new AdType { AdTypeId = 1, Name = "Type1", Description = "Description1" };
            context.AdTypes.Add(adType);
            await context.SaveChangesAsync();

            var controller = new AdTypesController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdType>(viewResult.ViewData.Model);
            Assert.Equal(adType, model);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenAdTypeDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AdTypesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenAdTypeExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var adType = new AdType { AdTypeId = 1, Name = "Type1", Description = "Description1" };
            context.AdTypes.Add(adType);
            await context.SaveChangesAsync();

            var controller = new AdTypesController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AdType>(viewResult.ViewData.Model);
            Assert.Equal(adType, model);
        }
    }
}