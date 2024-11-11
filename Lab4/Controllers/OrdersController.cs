using Lab4.Data;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Lab4.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AdvertisingDbContext _context;

        public OrdersController(AdvertisingDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
        {
            int pageSize = 10;
            var orders = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Location)
                .AsQueryable();

            switch (sortOrder)
            {
                case SortState.DateAsc:
                    orders = orders.OrderBy(s => s.OrderDate);
                    break;
                case SortState.DateDesc:
                    orders = orders.OrderByDescending(s => s.OrderDate);
                    break;
                case SortState.NameAsc:
                    orders = orders.OrderBy(s => s.Client.FirstName);
                    break;
                case SortState.NameDesc:
                    orders = orders.OrderByDescending(s => s.Client.FirstName);
                    break;
                case SortState.CostAsc:
                    orders = orders.OrderBy(s => s.TotalCost);
                    break;
                case SortState.CostDesc:
                    orders = orders.OrderByDescending(s => s.TotalCost);
                    break;
                default:
                    orders = orders.OrderBy(s => s.OrderId);
                    break;
            }

            var count = orders.Count();
            var items = orders.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new OrdersViewModel
            {
                Orders = items,
                PageViewModel = new PageViewModel(count, page, pageSize),
                OrderViewModel = new OrderViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder)
                }
            };

            return View(viewModel);
        }
    }
}
