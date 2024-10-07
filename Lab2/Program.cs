using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Lab2_RPBDIS
{
    class Program
    {
        static void Main(string[] args)
        {
            // 3.2.1. Выборка всех данных из таблицы, стоящей в схеме базы данных на стороне отношения «один»
            GetAllEmployees();

            // 3.2.2. Выборка данных из таблицы, стоящей в схеме базы данных на стороне отношения «один», отфильтрованные по определенному условию
            GetFilteredEmployees();

            // 3.2.3. Выборка данных, сгруппированных по любому из полей данных с выводом какого-либо итогового результата (min, max, avg, count или др.) по выбранному полю из таблицы, стоящей в схеме базы данных на стороне отношения «многие»
            GetGroupedOrders();

            // 3.2.4. Выборка данных из двух полей двух таблиц, связанных между собой отношением «один-ко-многим»
            GetOrdersWithClients();

            // 3.2.5. Выборка данных из двух таблиц, связанных между собой отношением «один-ко-многим» и отфильтрованным по некоторому условию, налагающему ограничения на значения одного или нескольких полей
            GetFilteredOrdersWithClients();

            // 3.2.6. Вставка данных в таблицы, стоящей на стороне отношения «Один»
            InsertEmployee();

            // 3.2.7. Вставка данных в таблицы, стоящей на стороне отношения «Многие»
            InsertOrder();

            // 3.2.8. Удаление данных из таблицы, стоящей на стороне отношения «Один»
            DeleteEmployee();

            // 3.2.9. Удаление данных из таблицы, стоящей на стороне отношения «Многие»
            DeleteOrder();

            // 3.2.10. Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных
            UpdateEmployees();
        }

        static void GetAllEmployees()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var employees = context.Employees.ToList();
                Console.WriteLine("All Employees:");
                foreach (var employee in employees)
                {
                    Console.WriteLine($"{employee.EmployeeId}: {employee.FirstName} {employee.LastName} - {employee.Position}");
                }
            }
        }

        static void GetFilteredEmployees()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var filteredEmployees = context.Employees
                    .Where(e => e.Position == "SEO Specialist")
                    .ToList();

                Console.WriteLine("Filtered Employees (Position = SEO Specialist):");
                foreach (var employee in filteredEmployees)
                {
                    Console.WriteLine($"{employee.EmployeeId}: {employee.FirstName} {employee.LastName} - {employee.Position}");
                }
            }
        }

        static void GetGroupedOrders()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var groupedOrders = context.Orders
                    .GroupBy(o => o.ClientId)
                    .Select(g => new
                    {
                        ClientId = g.Key,
                        TotalOrders = g.Count(),
                        TotalCost = g.Sum(o => o.TotalCost)
                    })
                    .ToList();

                Console.WriteLine("Grouped Orders by ClientId:");
                foreach (var group in groupedOrders)
                {
                    Console.WriteLine($"ClientId: {group.ClientId}, Total Orders: {group.TotalOrders}, Total Cost: {group.TotalCost}");
                }
            }
        }

        static void GetOrdersWithClients()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var ordersWithClients = context.Orders
                    .Select(o => new
                    {
                        o.OrderId,
                        o.OrderDate,
                        ClientName = o.Client.FirstName + " " + o.Client.LastName
                    })
                    .ToList();

                Console.WriteLine("Orders with Client Names:");
                foreach (var order in ordersWithClients)
                {
                    Console.WriteLine($"OrderId: {order.OrderId}, OrderDate: {order.OrderDate}, ClientName: {order.ClientName}");
                }
            }
        }

        static void GetFilteredOrdersWithClients()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var filteredOrdersWithClients = context.Orders
                    .Where(o => o.TotalCost > 995)
                    .Select(o => new
                    {
                        o.OrderId,
                        o.OrderDate,
                        ClientName = o.Client.FirstName + " " + o.Client.LastName
                    })
                    .ToList();

                Console.WriteLine("Filtered Orders with Client Names (TotalCost > 995):");
                foreach (var order in filteredOrdersWithClients)
                {
                    Console.WriteLine($"OrderId: {order.OrderId}, OrderDate: {order.OrderDate}, ClientName: {order.ClientName}");
                }
            }
        }

        static void InsertEmployee()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var newEmployee = new Employee
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Position = "Developer"
                };

                context.Employees.Add(newEmployee);
                context.SaveChanges();
                Console.WriteLine("New Employee Inserted.");
            }
        }

        static void InsertOrder()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var newOrder = new Order
                {
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    StartDate = DateOnly.FromDateTime(DateTime.Now),
                    EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                    ClientId = 1,
                    LocationId = 1,
                    EmployeeId = 142,
                    TotalCost = 500,
                    Paid = true
                };

                context.Orders.Add(newOrder);
                context.SaveChanges();
                Console.WriteLine("New Order Inserted.");
            }
        }

        static void DeleteEmployee()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var employeeToDelete = context.Employees.Include(e => e.Orders).FirstOrDefault(e => e.EmployeeId == 2);
                if (employeeToDelete != null)
                {
                    // Удаляем все связанные заказы
                    context.Orders.RemoveRange(employeeToDelete.Orders);
                    // Удаляем сотрудника
                    context.Employees.Remove(employeeToDelete);
                    context.SaveChanges();
                    Console.WriteLine("Employee Deleted.");
                }
            }
        }


        static void DeleteOrder()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var orderToDelete = context.Orders.FirstOrDefault(o => o.OrderId == 2);
                if (orderToDelete != null)
                {
                    context.Orders.Remove(orderToDelete);
                    context.SaveChanges();
                    Console.WriteLine("Order Deleted.");
                }
            }
        }

        static void UpdateEmployees()
        {
            using (var context = new AdvertisingAgencyDbContext())
            {
                var employeesToUpdate = context.Employees
                    .Where(e => e.Position == "Developer")
                    .ToList();

                foreach (var employee in employeesToUpdate)
                {
                    employee.Position = "Senior Developer";
                }

                context.SaveChanges();
                Console.WriteLine("Employees Updated.");
            }
        }
    }
}
