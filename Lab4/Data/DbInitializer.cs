using Lab4.Models;
using System;
using System.Linq;

namespace Lab4.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AdvertisingDbContext db)
        {
            db.Database.EnsureCreated();

            // Проверка занесены ли данные
            if (db.AdTypes.Any() && db.AdditionalServices.Any() && db.Clients.Any() && db.Employees.Any() && db.Locations.Any() && db.Orders.Any() && db.OrderServices.Any())
            {
                return;   // База данных инициализирована
            }

            Random randObj = new Random();

            // Заполнение таблицы AdTypes
            string[] adTypeNames = { "Billboard", "TV Ad", "Radio Ad", "Online Ad", "Print Ad" };
            string[] adTypeDescriptions = { "Outdoor advertising", "Television commercial", "Radio spot", "Internet banner", "Newspaper ad" };
            for (int i = 0; i < adTypeNames.Length; i++)
            {
                db.AdTypes.Add(new AdType { Name = adTypeNames[i], Description = adTypeDescriptions[i] });
            }
            db.SaveChanges();

            // Заполнение таблицы AdditionalServices
            string[] serviceNames = { "Design", "Copywriting", "SEO", "SMM", "Analytics" };
            string[] serviceDescriptions = { "Graphic design services", "Content creation", "Search engine optimization", "Social media marketing", "Data analysis" };
            decimal[] serviceCosts = { 500.00m, 300.00m, 400.00m, 250.00m, 600.00m };
            for (int i = 0; i < serviceNames.Length; i++)
            {
                db.AdditionalServices.Add(new AdditionalService { Name = serviceNames[i], Description = serviceDescriptions[i], Cost = serviceCosts[i] });
            }
            db.SaveChanges();

            // Заполнение таблицы Clients
            string[] clientFirstNames = { "John", "Jane", "Alice", "Bob", "Charlie" };
            string[] clientLastNames = { "Doe", "Smith", "Johnson", "Brown", "Davis" };
            string[] clientAddresses = { "123 Main St", "456 Elm St", "789 Oak St", "321 Pine St", "654 Maple St" };
            string[] clientPhoneNumbers = { "123-456-7890", "234-567-8901", "345-678-9012", "456-789-0123", "567-890-1234" };
            for (int i = 0; i < clientFirstNames.Length; i++)
            {
                db.Clients.Add(new Client { FirstName = clientFirstNames[i], LastName = clientLastNames[i], Address = clientAddresses[i], PhoneNumber = clientPhoneNumbers[i] });
            }
            db.SaveChanges();

            // Заполнение таблицы Employees
            string[] employeeFirstNames = { "Michael", "Sarah", "David", "Emily", "Daniel" };
            string[] employeeLastNames = { "Johnson", "Williams", "Brown", "Jones", "Miller" };
            string[] employeePositions = { "Manager", "Designer", "Developer", "Marketer", "Analyst" };
            for (int i = 0; i < employeeFirstNames.Length; i++)
            {
                db.Employees.Add(new Employee { FirstName = employeeFirstNames[i], LastName = employeeLastNames[i], Position = employeePositions[i] });
            }
            db.SaveChanges();

            // Заполнение таблицы Locations
            string[] locationNames = { "Location 1", "Location 2", "Location 3", "Location 4", "Location 5" };
            string[] locationDescriptions = { "Description 1", "Description 2", "Description 3", "Description 4", "Description 5" };
            string[] adDescriptions = { "Ad Description 1", "Ad Description 2", "Ad Description 3", "Ad Description 4", "Ad Description 5" };
            decimal[] locationCosts = { 1000.00m, 1500.00m, 2000.00m, 2500.00m, 3000.00m };
            for (int i = 0; i < locationNames.Length; i++)
            {
                int adTypeId = randObj.Next(1, db.AdTypes.Count() + 1);
                db.Locations.Add(new Location { Name = locationNames[i], LocationDescription = locationDescriptions[i], AdTypeId = adTypeId, AdDescription = adDescriptions[i], Cost = locationCosts[i] });
            }
            db.SaveChanges();

            // Заполнение таблицы Orders
            DateTime today = DateTime.Now.Date;
            for (int i = 0; i < 10; i++)
            {
                int clientId = randObj.Next(1, db.Clients.Count() + 1);
                int locationId = randObj.Next(1, db.Locations.Count() + 1);
                int employeeId = randObj.Next(1, db.Employees.Count() + 1);
                DateTime orderDate = today.AddDays(-i);
                DateTime startDate = orderDate.AddDays(1);
                DateTime endDate = orderDate.AddDays(5);
                decimal totalCost = randObj.Next(1000, 5000);
                bool paid = randObj.Next(2) == 0;
                db.Orders.Add(new Order { OrderDate = DateOnly.FromDateTime(orderDate), StartDate = DateOnly.FromDateTime(startDate), EndDate = DateOnly.FromDateTime(endDate), ClientId = clientId, LocationId = locationId, EmployeeId = employeeId, TotalCost = totalCost, Paid = paid });
            }
            db.SaveChanges();

            // Заполнение таблицы OrderServices
            for (int i = 0; i < 20; i++)
            {
                int orderId = randObj.Next(1, db.Orders.Count() + 1);
                int serviceId = randObj.Next(1, db.AdditionalServices.Count() + 1);
                int quantity = randObj.Next(1, 10);
                decimal totalCost = quantity * db.AdditionalServices.Find(serviceId).Cost.Value;
                db.OrderServices.Add(new OrderService { OrderId = orderId, ServiceId = serviceId, Quantity = quantity, TotalCost = totalCost });
            }
            db.SaveChanges();
        }
    }
}
