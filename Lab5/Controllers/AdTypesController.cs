//using Lab4.Data;
//using Lab4.Models;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Lab4.Controllers
//{
//    [Authorize] // Все действия требуют авторизации
//    public class AdTypesController : Controller
//    {
//        private readonly AdvertisingDbContext _context;
//        private readonly int pageSize = 10; // количество элементов на странице

//        public AdTypesController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        // GET: AdTypes
//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchName = "", string searchDescription = "")
//        {
//            IQueryable<AdType> adTypes = _context.AdTypes.AsQueryable();

//            // Сортировка и фильтрация данных
//            adTypes = Sort_Search(adTypes, sortOrder, searchName, searchDescription);

//            // Разбиение на страницы
//            var count = adTypes.Count();
//            adTypes = adTypes.Skip((page - 1) * pageSize).Take(pageSize);

//            var viewModel = new AdTypesViewModel
//            {
//                AdTypes = adTypes.ToList(),
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                AdTypeViewModel = new AdTypeViewModel
//                {
//                    SortViewModel = new SortViewModel(sortOrder),
//                    Name = searchName,
//                    Description = searchDescription
//                }
//            };

//            return View(viewModel);
//        }

//        // GET: AdTypes/Details/5
//        public async Task<IActionResult> Details(int id)
//        {
//            var adType = await _context.AdTypes.FindAsync(id);
//            if (adType == null)
//            {
//                return NotFound();
//            }
//            return View(adType);
//        }

//        // Создание типа рекламы
//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Name,Description")] AdType adType)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(adType);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(adType);
//        }

//        // Редактирование типа рекламы
//        [Authorize(Roles = "Admin")] // Доступ только для администраторов
//        public async Task<IActionResult> Edit(int id)
//        {
//            var adType = await _context.AdTypes.FindAsync(id);
//            if (adType == null)
//            {
//                return NotFound();
//            }
//            return View(adType);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Admin")] // Доступ только для администраторов
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("AdTypeId,Name,Description")] AdType adType)
//        {
//            if (id != adType.AdTypeId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(adType);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!AdTypeExists(adType.AdTypeId))
//                    {
//                        return NotFound();
//                    }
//                    throw;
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            return View(adType);
//        }

//        // Удаление типа рекламы
//        [Authorize(Roles = "Admin")] // Доступ только для администраторов
//        public async Task<IActionResult> Delete(int id)
//        {
//            var adType = await _context.AdTypes.FindAsync(id);
//            if (adType == null)
//            {
//                return NotFound();
//            }
//            return View(adType);
//        }

//        [HttpPost, ActionName("Delete")]
//        [Authorize(Roles = "Admin")] // Доступ только для администраторов
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var adType = await _context.AdTypes.FindAsync(id);
//            if (adType != null)
//            {
//                _context.AdTypes.Remove(adType);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool AdTypeExists(int id)
//        {
//            return _context.AdTypes.Any(e => e.AdTypeId == id);
//        }

//        // Сортировка и фильтрация данных
//        private static IQueryable<AdType> Sort_Search(IQueryable<AdType> adTypes, SortState sortOrder, string searchName, string searchDescription)
//        {
//            if (!string.IsNullOrEmpty(searchName))
//            {
//                adTypes = adTypes.Where(s => s.Name.Contains(searchName));
//            }

//            if (!string.IsNullOrEmpty(searchDescription))
//            {
//                adTypes = adTypes.Where(s => s.Description.Contains(searchDescription));
//            }

//            switch (sortOrder)
//            {
//                case SortState.NameAsc:
//                    adTypes = adTypes.OrderBy(s => s.Name);
//                    break;
//                case SortState.NameDesc:
//                    adTypes = adTypes.OrderByDescending(s => s.Name);
//                    break;
//                case SortState.DescriptionAsc:
//                    adTypes = adTypes.OrderBy(s => s.Description);
//                    break;
//                case SortState.DescriptionDesc:
//                    adTypes = adTypes.OrderByDescending(s => s.Description);
//                    break;
//                default:
//                    adTypes = adTypes.OrderBy(s => s.AdTypeId);
//                    break;
//            }

//            return adTypes;
//        }
//    }
//}
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
    public class AdTypesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

        public AdTypesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: AdTypes
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchName = "", string searchDescription = "")
        {
            IQueryable<AdType> adTypes = _context.AdTypes.AsQueryable();

            // Сортировка и фильтрация данных
            adTypes = Sort_Search(adTypes, sortOrder, searchName, searchDescription);

            // Разбиение на страницы
            var count = adTypes.Count();
            adTypes = adTypes.Skip((page - 1) * pageSize).Take(pageSize);

            var viewModel = new AdTypesViewModel
            {
                AdTypes = adTypes.ToList(),
                PageViewModel = new PageViewModel(count, page, pageSize),
                AdTypeViewModel = new AdTypeViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder),
                    Name = searchName,
                    Description = searchDescription
                }
            };

            return View(viewModel);
        }

        // GET: AdTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var adType = await _context.AdTypes.FindAsync(id);
            if (adType == null)
            {
                return NotFound();
            }
            return View(adType);
        }

        // Создание типа рекламы
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] AdType adType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adType);
        }

        // Редактирование типа рекламы
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Edit(int id)
        {
            var adType = await _context.AdTypes.FindAsync(id);
            if (adType == null)
            {
                return NotFound();
            }
            return View(adType);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdTypeId,Name,Description")] AdType adType)
        {
            if (id != adType.AdTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdTypeExists(adType.AdTypeId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(adType);
        }

        // Удаление типа рекламы
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Delete(int id)
        {
            var adType = await _context.AdTypes.FindAsync(id);
            if (adType == null)
            {
                return NotFound();
            }
            return View(adType);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adType = await _context.AdTypes.FindAsync(id);
            if (adType != null)
            {
                if (_context.Locations.Any(l => l.AdTypeId == id))
                {
                    return View("CannotDelete", "Невозможно удалить этот тип рекламы, так как он используется в других записях.");
                }

                _context.AdTypes.Remove(adType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AdTypeExists(int id)
        {
            return _context.AdTypes.Any(e => e.AdTypeId == id);
        }

        // Сортировка и фильтрация данных
        private static IQueryable<AdType> Sort_Search(IQueryable<AdType> adTypes, SortState sortOrder, string searchName, string searchDescription)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                adTypes = adTypes.Where(s => s.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchDescription))
            {
                adTypes = adTypes.Where(s => s.Description.Contains(searchDescription));
            }

            switch (sortOrder)
            {
                case SortState.NameAsc:
                    adTypes = adTypes.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    adTypes = adTypes.OrderByDescending(s => s.Name);
                    break;
                case SortState.DescriptionAsc:
                    adTypes = adTypes.OrderBy(s => s.Description);
                    break;
                case SortState.DescriptionDesc:
                    adTypes = adTypes.OrderByDescending(s => s.Description);
                    break;
                default:
                    adTypes = adTypes.OrderBy(s => s.AdTypeId);
                    break;
            }

            return adTypes;
        }
    }
}
