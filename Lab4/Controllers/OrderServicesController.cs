using Lab4.Data;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Lab4.Controllers
{
    public class OrderServicesController : Controller
    {
        private readonly AdvertisingDbContext _context;

        public OrderServicesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
        {
            int pageSize = 10;
            var orderServices = _context.OrderServices
                .Include(os => os.Order)
                .Include(os => os.Service)
                .AsQueryable();

            switch (sortOrder)
            {
                case SortState.NameAsc:
                    orderServices = orderServices.OrderBy(s => s.Service.Name);
                    break;
                case SortState.NameDesc:
                    orderServices = orderServices.OrderByDescending(s => s.Service.Name);
                    break;
                case SortState.CostAsc:
                    orderServices = orderServices.OrderBy(s => s.TotalCost);
                    break;
                case SortState.CostDesc:
                    orderServices = orderServices.OrderByDescending(s => s.TotalCost);
                    break;
                default:
                    orderServices = orderServices.OrderBy(s => s.OrderServiceId);
                    break;
            }

            var count = orderServices.Count();
            var items = orderServices.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new OrderServicesViewModel
            {
                OrderServices = items,
                PageViewModel = new PageViewModel(count, page, pageSize),
                OrderServiceViewModel = new OrderServiceViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder)
                }
            };

            return View(viewModel);
        }
    }
}
