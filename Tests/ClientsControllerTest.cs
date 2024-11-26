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
    public class ClientsControllerTest
    {
        private AdvertisingDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AdvertisingDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{System.Guid.NewGuid()}")
                .Options;

            return new AdvertisingDbContext(options);
        }


        [Fact]
        public async Task Details_ReturnsNotFound_WhenClientDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new ClientsController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithClient()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var client = new Client { ClientId = 1, FirstName = "John", LastName = "Doe" };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var controller = new ClientsController(context);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Client>(viewResult.ViewData.Model);
            Assert.Equal(client, model);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsValid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new ClientsController(context);
            var client = new Client { FirstName = "John", LastName = "Doe", Address = "123 Main St", PhoneNumber = "1234567890" };

            // Act
            var result = await controller.Create(client);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.Equal(1, await context.Clients.CountAsync());
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new ClientsController(context);
            controller.ModelState.AddModelError("FirstName", "Required");

            var client = new Client();

            // Act
            var result = await controller.Create(client);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(client, viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenClientDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new ClientsController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WhenClientExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var client = new Client { ClientId = 1, FirstName = "John", LastName = "Doe" };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var controller = new ClientsController(context);

            // Act
            var result = await controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Client>(viewResult.ViewData.Model);
            Assert.Equal(client, model);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenClientDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new ClientsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenClientExists()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var client = new Client { ClientId = 1, FirstName = "John", LastName = "Doe" };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var controller = new ClientsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Client>(viewResult.ViewData.Model);
            Assert.Equal(client, model);
        }
    }
}