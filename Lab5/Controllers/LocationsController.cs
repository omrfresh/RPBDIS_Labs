using Lab4.Data;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab4.Controllers
{
    [Authorize] // Все действия требуют авторизации
    public class LocationsController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

        public LocationsController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchName = "", string searchDescription = "")
        {
            var locations = _context.Locations
                .Include(l => l.AdType)
                .AsQueryable();

            // Сортировка и фильтрация данных
            locations = Sort_Search(locations, sortOrder, searchName, searchDescription);

            // Разбиение на страницы
            var count = locations.Count();
            locations = locations.Skip((page - 1) * pageSize).Take(pageSize);

            var viewModel = new LocationsViewModel
            {
                Locations = locations.ToList(),
                PageViewModel = new PageViewModel(count, page, pageSize),
                LocationViewModel = new LocationViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder),
                    Name = searchName,
                    LocationDescription = searchDescription
                }
            };

            return View(viewModel);
        }

        // GET: Locations/Details/5
        [Authorize] // Доступ для всех авторизованных пользователей
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.AdType)
                .SingleOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public IActionResult Create()
        {
            ViewData["AdTypeId"] = new SelectList(_context.AdTypes, "AdTypeId", "Name");
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,Name,LocationDescription,Cost,AdTypeId,AdDescription")] Location location)
        {
            if (!ModelState.IsValid)
            {
                ViewData["AdTypeId"] = new SelectList(_context.AdTypes, "AdTypeId", "Name", location.AdTypeId);
                return View(location);
            }
            else
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Locations/Edit/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.SingleOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["AdTypeId"] = new SelectList(_context.AdTypes, "AdTypeId", "Name", location.AdTypeId);
            return View(location);
        }

        // POST: Locations/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,Name,LocationDescription,Cost,AdTypeId,AdDescription")] Location location)
        {
            if (id != location.LocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.LocationId))
                    {
                        return NotFound();
                    }
                    throw; // Если произошла ошибка, выбросить исключение
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AdTypeId"] = new SelectList(_context.AdTypes, "AdTypeId", "Name", location.AdTypeId);
            return View(location);
        }

        // GET: Locations/Delete/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .Include(l => l.AdType)
                .SingleOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.SingleOrDefaultAsync(m => m.LocationId == id);
            if (location != null)
            {
                if (_context.Orders.Any(o => o.LocationId == id))
                {
                    return View("CannotDelete", "Невозможно удалить эту локацию, так как она используется в других записях.");
                }

                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }

        // Сортировка и фильтрация данных
        private static IQueryable<Location> Sort_Search(IQueryable<Location> locations, SortState sortOrder, string searchName, string searchDescription)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    locations = locations.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    locations = locations.OrderByDescending(s => s.Name);
                    break;
                case SortState.DescriptionAsc:
                    locations = locations.OrderBy(s => s.LocationDescription);
                    break;
                case SortState.DescriptionDesc:
                    locations = locations.OrderByDescending(s => s.LocationDescription);
                    break;
                case SortState.CostAsc:
                    locations = locations.OrderBy(s => s.Cost);
                    break;
                case SortState.CostDesc:
                    locations = locations.OrderByDescending(s => s.Cost);
                    break;
                default:
                    locations = locations.OrderBy(s => s.LocationId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                locations = locations.Where(l => l.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchDescription))
            {
                locations = locations.Where(l => l.LocationDescription.Contains(searchDescription));
            }

            return locations;
        }
    }
}
