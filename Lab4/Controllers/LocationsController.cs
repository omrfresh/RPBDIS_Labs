//using Lab4.Data;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Lab4.Controllers
//{
//    public class LocationsController : Controller
//    {
//        private readonly AdvertisingDbContext _context;

//        public LocationsController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
//        {
//            int pageSize = 10;
//            var locations = _context.Locations
//                .Include(l => l.AdType) // Подгружаем данные AdType
//                .AsQueryable();

//            switch (sortOrder)
//            {
//                case SortState.NameAsc:
//                    locations = locations.OrderBy(s => s.Name);
//                    break;
//                case SortState.NameDesc:
//                    locations = locations.OrderByDescending(s => s.Name);
//                    break;
//                case SortState.DescriptionAsc:
//                    locations = locations.OrderBy(s => s.LocationDescription);
//                    break;
//                case SortState.DescriptionDesc:
//                    locations = locations.OrderByDescending(s => s.LocationDescription);
//                    break;
//                case SortState.CostAsc:
//                    locations = locations.OrderBy(s => s.Cost);
//                    break;
//                case SortState.CostDesc:
//                    locations = locations.OrderByDescending(s => s.Cost);
//                    break;
//                default:
//                    locations = locations.OrderBy(s => s.LocationId);
//                    break;
//            }

//            var count = locations.Count();
//            var items = locations.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new LocationsViewModel
//            {
//                Locations = items,
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                LocationViewModel = new LocationViewModel
//                {
//                    SortViewModel = new SortViewModel(sortOrder)
//                }
//            };

//            return View(viewModel);
//        }
//    }
//}
using Lab4.Data;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Lab4.Models;


namespace Lab4.Controllers
{
    public class LocationsController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public LocationsController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchName = "", string searchDescription = "")
        {
            IQueryable<Location> locations = _context.Locations.Include(l => l.AdType).AsQueryable();

            // Сортировка и фильтрация данных
            locations = Sort_Search(locations, sortOrder, searchName, searchDescription);

            // Разбиение на страницы
            var count = locations.Count();
            locations = locations.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            var viewModel = new LocationsViewModel
            {
                Locations = locations,
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

        // POST: Locations
        [HttpPost]
        public IActionResult Index(LocationViewModel locationViewModel, int page = 1)
        {
            if (locationViewModel == null)
            {
                locationViewModel = new LocationViewModel();
            }

            if (locationViewModel.SortViewModel == null)
            {
                locationViewModel.SortViewModel = new SortViewModel(SortState.No);
            }

            return RedirectToAction("Index", new { page, sortOrder = locationViewModel.SortViewModel.CurrentState, searchName = locationViewModel.Name, searchDescription = locationViewModel.LocationDescription });
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
