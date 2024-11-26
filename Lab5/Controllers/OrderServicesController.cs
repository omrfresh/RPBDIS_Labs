//using Lab4.Data;
//using Lab4.Models;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Lab4.Controllers
//{
//    public class OrderServicesController : Controller
//    {
//        private readonly AdvertisingDbContext _context;
//        private readonly int pageSize = 10; // количество элементов на странице

//        public OrderServicesController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        // GET: OrderServices
//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
//        {
//            var orderServices = _context.OrderServices
//                .Include(os => os.Order)
//                .Include(os => os.Service)
//                .AsQueryable();

//            // Сортировка
//            orderServices = Sort(orderServices, sortOrder);

//            // Разбиение на страницы
//            var count = orderServices.Count();
//            var items = orderServices.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new OrderServicesViewModel
//            {
//                OrderServices = items,
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                OrderServiceViewModel = new OrderServiceViewModel
//                {
//                    SortViewModel = new SortViewModel(sortOrder)
//                }
//            };

//            return View(viewModel);
//        }

//        // GET: OrderServices/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var orderService = await _context.OrderServices
//                .Include(os => os.Order)
//                .Include(os => os.Service)
//                .SingleOrDefaultAsync(m => m.OrderServiceId == id);

//            if (orderService == null)
//            {
//                return NotFound();
//            }

//            return View(orderService);
//        }

//        // GET: OrderServices/Create
//        public IActionResult Create()
//        {
//            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
//            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name");
//            return View();
//        }

//        // POST: OrderServices/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("OrderServiceId,OrderId,ServiceId,Quantity,TotalCost")] OrderService orderService)
//        {
//            if (ModelState.IsValid)
//            {
//                var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == orderService.OrderId);
//                var serviceExists = await _context.AdditionalServices.AnyAsync(s => s.AdditionalServiceId == orderService.ServiceId);

//                if (!orderExists)
//                {
//                    ModelState.AddModelError("OrderId", "Заказ с указанным ID не существует.");
//                }

//                if (!serviceExists)
//                {
//                    ModelState.AddModelError("ServiceId", "Услуга с указанным ID не существует.");
//                }

//                if (orderExists && serviceExists)
//                {
//                    _context.Add(orderService);
//                    await _context.SaveChangesAsync();
//                    return RedirectToAction(nameof(Index));
//                }
//            }

//            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderService.OrderId);
//            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", orderService.ServiceId);
//            return View(orderService);
//        }

//        // GET: OrderServices/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var orderService = await _context.OrderServices.FindAsync(id);
//            if (orderService == null)
//            {
//                return NotFound();
//            }

//            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderService.OrderId);
//            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", orderService.ServiceId);
//            return View(orderService);
//        }

//        // POST: OrderServices/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("OrderServiceId,OrderId,ServiceId,Quantity,TotalCost")] OrderService orderService)
//        {
//            if (id != orderService.OrderServiceId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(orderService);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!OrderServiceExists(orderService.OrderServiceId))
//                    {
//                        return NotFound();
//                    }
//                    throw;
//                }
//                return RedirectToAction(nameof(Index));
//            }

//            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderService.OrderId);
//            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", orderService.ServiceId);
//            return View(orderService);
//        }

//        // GET: OrderServices/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var orderService = await _context.OrderServices
//                .Include(os => os.Order)
//                .Include(os => os.Service)
//                .SingleOrDefaultAsync(m => m.OrderServiceId == id);
//            if (orderService == null)
//            {
//                return NotFound();
//            }

//            return View(orderService);
//        }

//        // POST: OrderServices/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var orderService = await _context.OrderServices.FindAsync(id);
//            if (orderService != null)
//            {
//                _context.OrderServices.Remove(orderService);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool OrderServiceExists(int id)
//        {
//            return _context.OrderServices.Any(e => e.OrderServiceId == id);
//        }

//        private static IQueryable<OrderService> Sort(IQueryable<OrderService> orderServices, SortState sortOrder)
//        {
//            switch (sortOrder)
//            {
//                case SortState.NameAsc:
//                    return orderServices.OrderBy(os => os.Service.Name);
//                case SortState.NameDesc:
//                    return orderServices.OrderByDescending(os => os.Service.Name);
//                case SortState.CostAsc:
//                    return orderServices.OrderBy(os => os.TotalCost);
//                case SortState.CostDesc:
//                    return orderServices.OrderByDescending(os => os.TotalCost);
//                default:
//                    return orderServices.OrderBy(os => os.OrderServiceId);
//            }
//        }
//    }
//}
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
    [Authorize]
    public class OrderServicesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

        public OrderServicesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        private bool OrderServiceExists(int id)
        {
            return _context.OrderServices.Any(e => e.OrderServiceId == id);
        }

        // GET: OrderServices
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
        {
            var orderServices = _context.OrderServices
                .Include(os => os.Order)
                .Include(os => os.Service)
                .AsQueryable();

            // Сортировка
            orderServices = Sort(orderServices, sortOrder);

            // Разбиение на страницы
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

        // GET: OrderServices/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _context.OrderServices
                .Include(os => os.Order)
                .Include(os => os.Service)
                .SingleOrDefaultAsync(m => m.OrderServiceId == id);

            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // GET: OrderServices/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId");
            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name");
            return View();
        }

        // POST: OrderServices/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderServiceId,OrderId,ServiceId,Quantity,TotalCost")] OrderService orderService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderService.OrderId);
            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", orderService.ServiceId);
            return View(orderService);
        }

        // GET: OrderServices/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _context.OrderServices.FindAsync(id);
            if (orderService == null)
            {
                return NotFound();
            }

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderService.OrderId);
            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", orderService.ServiceId);
            return View(orderService);
        }

        // POST: OrderServices/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderServiceId,OrderId,ServiceId,Quantity,TotalCost")] OrderService orderService)
        {
            if (id != orderService.OrderServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderServiceExists(orderService.OrderServiceId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "OrderId", orderService.OrderId);
            ViewData["ServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", orderService.ServiceId);
            return View(orderService);
        }

        // GET: OrderServices/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _context.OrderServices
                .Include(os => os.Order)
                .Include(os => os.Service)
                .SingleOrDefaultAsync(m => m.OrderServiceId == id);
            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // POST: OrderServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderService = await _context.OrderServices.FindAsync(id);
            if (orderService != null)
            {
                _context.OrderServices.Remove(orderService);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private static IQueryable<OrderService> Sort(IQueryable<OrderService> orderServices, SortState sortOrder)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    return orderServices.OrderBy(os => os.Service.Name);
                case SortState.NameDesc:
                    return orderServices.OrderByDescending(os => os.Service.Name);
                case SortState.CostAsc:
                    return orderServices.OrderBy(os => os.TotalCost);
                case SortState.CostDesc:
                    return orderServices.OrderByDescending(os => os.TotalCost);
                default:
                    return orderServices.OrderBy(os => os.OrderServiceId);
            }
        }
    }
}
