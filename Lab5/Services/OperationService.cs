using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Lab4.Services
{
    // Класс выборки 10 записей из всех таблиц
    public class OperationService(AdvertisingDbContext context) : IOperationService
    {
        private readonly AdvertisingDbContext _context = context;

        public HomeViewModel GetHomeViewModel(int numberRows = 10)
        {
            List<AdType> adTypes = _context.AdTypes.Take(numberRows).ToList();
            List<AdditionalService> additionalServices = _context.AdditionalServices.Take(numberRows).ToList();
            List<Client> clients = _context.Clients.Take(numberRows).ToList();
            List<Employee> employees = _context.Employees.Take(numberRows).ToList();
            List<Location> locations = _context.Locations.Include(l => l.AdType).Take(numberRows).ToList();
            List<Order> orders = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Location)
                .Take(numberRows).ToList();
            List<OrderService> orderServices = _context.OrderServices
                .Include(os => os.Order)
                .Include(os => os.Service)
                .Take(numberRows).ToList();

            HomeViewModel homeViewModel = new HomeViewModel
            {
                AdTypes = adTypes,
                AdditionalServices = additionalServices,
                Clients = clients,
                Employees = employees,
                Locations = locations,
                Orders = orders,
                OrderServices = orderServices
            };

            return homeViewModel;
        }
    }
}
