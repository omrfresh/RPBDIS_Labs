using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Controllers
{
    [Authorize] // Все действия требуют авторизации
    public class AdditionalServicesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

        public AdditionalServicesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: AdditionalServices
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchName = "", string searchDescription = "")
        {
            IQueryable<AdditionalService> additionalServices = _context.AdditionalServices.AsQueryable();

            // Сортировка и фильтрация данных
            additionalServices = Sort_Search(additionalServices, sortOrder, searchName, searchDescription);

            // Разбиение на страницы
            var count = additionalServices.Count();
            additionalServices = additionalServices.Skip((page - 1) * pageSize).Take(pageSize);

            var viewModel = new AdditionalServicesViewModel
            {
                AdditionalServices = additionalServices.ToList(),
                PageViewModel = new PageViewModel(count, page, pageSize),
                AdditionalServiceViewModel = new AdditionalServiceViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder),
                    Name = searchName,
                    Description = searchDescription
                }
            };

            return View(viewModel);
        }

        // GET: AdditionalServices/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var additionalService = await _context.AdditionalServices.FindAsync(id);
            if (additionalService == null)
            {
                return NotFound();
            }
            return View(additionalService);
        }

        // GET: AdditionalServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdditionalServices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Cost")] AdditionalService additionalService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(additionalService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(additionalService);
        }

        // GET: AdditionalServices/Edit/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Edit(int id)
        {
            var additionalService = await _context.AdditionalServices.FindAsync(id);
            if (additionalService == null)
            {
                return NotFound();
            }
            return View(additionalService);
        }

        // POST: AdditionalServices/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdditionalServiceId,Name,Description,Cost")] AdditionalService additionalService)
        {
            if (id != additionalService.AdditionalServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(additionalService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdditionalServiceExists(additionalService.AdditionalServiceId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(additionalService);
        }

        // GET: AdditionalServices/Delete/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Delete(int id)
        {
            var additionalService = await _context.AdditionalServices.FindAsync(id);
            if (additionalService == null)
            {
                return NotFound();
            }
            return View(additionalService);
        }

        // POST: AdditionalServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var additionalService = await _context.AdditionalServices.FindAsync(id);
            if (additionalService != null)
            {
                if (_context.OrderServices.Any(os => os.ServiceId == id))
                {
                    return View("CannotDelete", "Невозможно удалить эту услугу, так как она используется в других записях.");
                }

                _context.AdditionalServices.Remove(additionalService);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Сортировка и фильтрация данных
        private static IQueryable<AdditionalService> Sort_Search(IQueryable<AdditionalService> services, SortState sortOrder, string searchName, string searchDescription)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                services = services.Where(s => s.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchDescription))
            {
                services = services.Where(s => s.Description.Contains(searchDescription));
            }

            switch (sortOrder)
            {
                case SortState.NameAsc:
                    services = services.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    services = services.OrderByDescending(s => s.Name);
                    break;
                case SortState.DescriptionAsc:
                    services = services.OrderBy(s => s.Description);
                    break;
                case SortState.DescriptionDesc:
                    services = services.OrderByDescending(s => s.Description);
                    break;
                case SortState.CostAsc:
                    services = services.OrderBy(s => s.Cost);
                    break;
                case SortState.CostDesc:
                    services = services.OrderByDescending(s => s.Cost);
                    break;
                default:
                    services = services.OrderBy(s => s.AdditionalServiceId);
                    break;
            }

            return services;
        }

        private bool AdditionalServiceExists(int id)
        {
            return _context.AdditionalServices.Any(e => e.AdditionalServiceId == id);
        }
    }
}
