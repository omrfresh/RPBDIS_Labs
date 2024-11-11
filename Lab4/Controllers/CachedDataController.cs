using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Lab4.Controllers
{
    public class CachedDataController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CachedDataController(AdvertisingDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // Кэширование данных и отображение их в отдельном окне
        [ResponseCache(Duration = 2 * 18 + 240)]
        public IActionResult Index()
        {
            int numberRows = 10;
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

            // Сохранение данных в кэш
            _memoryCache.Set("CachedData", homeViewModel);

            return View(homeViewModel);
        }
    }
}
