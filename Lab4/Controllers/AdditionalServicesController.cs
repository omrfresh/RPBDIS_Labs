//using Lab4.Data;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Lab4.Controllers
//{
//    public class AdditionalServicesController : Controller
//    {
//        private readonly AdvertisingDbContext _context;

//        public AdditionalServicesController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
//        {
//            int pageSize = 10;
//            var additionalServices = _context.AdditionalServices.AsQueryable();

//            switch (sortOrder)
//            {
//                case SortState.NameAsc:
//                    additionalServices = additionalServices.OrderBy(s => s.Name);
//                    break;
//                case SortState.NameDesc:
//                    additionalServices = additionalServices.OrderByDescending(s => s.Name);
//                    break;
//                case SortState.DescriptionAsc:
//                    additionalServices = additionalServices.OrderBy(s => s.Description);
//                    break;
//                case SortState.DescriptionDesc:
//                    additionalServices = additionalServices.OrderByDescending(s => s.Description);
//                    break;
//                case SortState.CostAsc:
//                    additionalServices = additionalServices.OrderBy(s => s.Cost);
//                    break;
//                case SortState.CostDesc:
//                    additionalServices = additionalServices.OrderByDescending(s => s.Cost);
//                    break;
//                default:
//                    additionalServices = additionalServices.OrderBy(s => s.AdditionalServiceId);
//                    break;
//            }

//            var count = additionalServices.Count();
//            var items = additionalServices.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new AdditionalServicesViewModel
//            {
//                AdditionalServices = items,
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                AdditionalServiceViewModel = new AdditionalServiceViewModel
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
    public class AdditionalServicesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public AdditionalServicesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: AdditionalServices
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchName = "", string searchDescription = "")
        {
            IQueryable<AdditionalService> services = _context.AdditionalServices.AsQueryable();

            // Сортировка и фильтрация данных
            services = Sort_Search(services, sortOrder, searchName, searchDescription);

            // Разбиение на страницы
            var count = services.Count();
            services = services.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            var viewModel = new AdditionalServicesViewModel
            {
                AdditionalServices = services,
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

        // POST: AdditionalServices
        [HttpPost]
        public IActionResult Index(AdditionalServiceViewModel serviceViewModel, int page = 1)
        {
            if (serviceViewModel == null)
            {
                serviceViewModel = new AdditionalServiceViewModel();
            }

            if (serviceViewModel.SortViewModel == null)
            {
                serviceViewModel.SortViewModel = new SortViewModel(SortState.No);
            }

            return RedirectToAction("Index", new { page, sortOrder = serviceViewModel.SortViewModel.CurrentState, searchName = serviceViewModel.Name, searchDescription = serviceViewModel.Description });
        }

        // Сортировка и фильтрация данных
        private static IQueryable<AdditionalService> Sort_Search(IQueryable<AdditionalService> services, SortState sortOrder, string searchName, string searchDescription)
        {
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

            if (!string.IsNullOrEmpty(searchName))
            {
                services = services.Where(s => s.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchDescription))
            {
                services = services.Where(s => s.Description.Contains(searchDescription));
            }

            return services;
        }
    }
}
