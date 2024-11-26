using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [Authorize] // Все действия требуют авторизации
    public class OrdersController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

        public OrdersController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
        {
            var orders = _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Location)
                .AsQueryable();

            // Сортировка данных
            orders = SortOrders(orders, sortOrder);

            // Разбиение на страницы
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

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Location)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,StartDate,EndDate,ClientId,LocationId,EmployeeId,TotalCost,Paid")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", order.EmployeeId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", order.LocationId);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", order.EmployeeId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", order.LocationId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,StartDate,EndDate,ClientId,LocationId,EmployeeId,TotalCost,Paid")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", order.EmployeeId);
            ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name", order.LocationId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Location)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Сортировка данных
        private IQueryable<Order> SortOrders(IQueryable<Order> orders, SortState sortOrder)
        {
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
            return orders;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
