using Lab6.Data;
using Lab6.Models;
using System.Linq;

namespace Lab6
{
    public static class DbInitializer
    {
        public static void Initialize(AdvertisingDbContext context)
        {
            context.Database.EnsureCreated();

            // Проверка наличия данных в базе
            if (context.AdTypes.Any())
            {
                return;   // База данных уже содержит данные
            }

            // Добавление начальных данных
            var adTypes = new AdType[]
            {
                new AdType { Name = "Тип рекламы 1", Description = "Описание типа рекламы 1" },
                new AdType { Name = "Тип рекламы 2", Description = "Описание типа рекламы 2" }
            };
            context.AdTypes.AddRange(adTypes);

            var locations = new Location[]
            {
                new Location { Name = "Локация 1", LocationDescription = "Описание локации 1", AdTypeId = 1, Cost = 100 },
                new Location { Name = "Локация 2", LocationDescription = "Описание локации 2", AdTypeId = 2, Cost = 200 }
            };
            context.Locations.AddRange(locations);

            var clients = new Client[]
            {
                new Client { FirstName = "Иван", LastName = "Иванов", Address = "Адрес 1", PhoneNumber = "1234567890" },
                new Client { FirstName = "Петр", LastName = "Петров", Address = "Адрес 2", PhoneNumber = "0987654321" }
            };
            context.Clients.AddRange(clients);

            context.SaveChanges();
        }
    }
}
