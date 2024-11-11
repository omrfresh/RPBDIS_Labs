//using Lab4.Data;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Lab4.Controllers
//{
//    public class AdTypesController : Controller
//    {
//        private readonly AdvertisingDbContext _context;

//        public AdTypesController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
//        {
//            int pageSize = 10;
//            var adTypes = _context.AdTypes.AsQueryable();

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

//            var count = adTypes.Count();
//            var items = adTypes.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new AdTypesViewModel
//            {
//                AdTypes = items,
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                AdTypeViewModel = new AdTypeViewModel
//                {
//                    SortViewModel = new SortViewModel(sortOrder)
//                }
//            };

//            return View(viewModel);
//        }
//    }
//}
using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Lab4.Controllers
{
    public class AdTypesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

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

            // Формирование модели для передачи представлению
            var viewModel = new AdTypesViewModel
            {
                AdTypes = adTypes,
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

        // POST: AdTypes
        [HttpPost]
        public IActionResult Index(AdTypeViewModel adTypeViewModel, int page = 1)
        {
            if (adTypeViewModel == null)
            {
                adTypeViewModel = new AdTypeViewModel();
            }

            if (adTypeViewModel.SortViewModel == null)
            {
                adTypeViewModel.SortViewModel = new SortViewModel(SortState.No);
            }

            return RedirectToAction("Index", new { page, sortOrder = adTypeViewModel.SortViewModel.CurrentState, searchName = adTypeViewModel.Name, searchDescription = adTypeViewModel.Description });
        }

        // Сортировка и фильтрация данных
        private static IQueryable<AdType> Sort_Search(IQueryable<AdType> adTypes, SortState sortOrder, string searchName, string searchDescription)
        {
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

            if (!string.IsNullOrEmpty(searchName))
            {
                adTypes = adTypes.Where(s => s.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchDescription))
            {
                adTypes = adTypes.Where(s => s.Description.Contains(searchDescription));
            }

            return adTypes;
        }
    }
}
