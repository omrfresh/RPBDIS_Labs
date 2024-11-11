//using Lab4.Data;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Lab4.Controllers
//{
//    public class ClientsController : Controller
//    {
//        private readonly AdvertisingDbContext _context;

//        public ClientsController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
//        {
//            int pageSize = 10;
//            var clients = _context.Clients.AsQueryable();

//            switch (sortOrder)
//            {
//                case SortState.NameAsc:
//                    clients = clients.OrderBy(s => s.FirstName);
//                    break;
//                case SortState.NameDesc:
//                    clients = clients.OrderByDescending(s => s.FirstName);
//                    break;
//                case SortState.DescriptionAsc:
//                    clients = clients.OrderBy(s => s.LastName);
//                    break;
//                case SortState.DescriptionDesc:
//                    clients = clients.OrderByDescending(s => s.LastName);
//                    break;
//                case SortState.CostAsc:
//                    clients = clients.OrderBy(s => s.Address);
//                    break;
//                case SortState.CostDesc:
//                    clients = clients.OrderByDescending(s => s.Address);
//                    break;
//                default:
//                    clients = clients.OrderBy(s => s.ClientId);
//                    break;
//            }

//            var count = clients.Count();
//            var items = clients.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new ClientsViewModel
//            {
//                Clients = items,
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                ClientViewModel = new ClientViewModel
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
    public class ClientsController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public ClientsController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchFirstName = "", string searchLastName = "")
        {
            IQueryable<Client> clients = _context.Clients.AsQueryable();

            // Сортировка и фильтрация данных
            clients = Sort_Search(clients, sortOrder, searchFirstName, searchLastName);

            // Разбиение на страницы
            var count = clients.Count();
            clients = clients.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            var viewModel = new ClientsViewModel
            {
                Clients = clients,
                PageViewModel = new PageViewModel(count, page, pageSize),
                ClientViewModel = new ClientViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder),
                    FirstName = searchFirstName,
                    LastName = searchLastName
                }
            };

            return View(viewModel);
        }

        // POST: Clients
        [HttpPost]
        public IActionResult Index(ClientViewModel clientViewModel, int page = 1)
        {
            if (clientViewModel == null)
            {
                clientViewModel = new ClientViewModel();
            }

            if (clientViewModel.SortViewModel == null)
            {
                clientViewModel.SortViewModel = new SortViewModel(SortState.No);
            }

            return RedirectToAction("Index", new { page, sortOrder = clientViewModel.SortViewModel.CurrentState, searchFirstName = clientViewModel.FirstName, searchLastName = clientViewModel.LastName });
        }

        // Сортировка и фильтрация данных
        private static IQueryable<Client> Sort_Search(IQueryable<Client> clients, SortState sortOrder, string searchFirstName, string searchLastName)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    clients = clients.OrderBy(s => s.FirstName);
                    break;
                case SortState.NameDesc:
                    clients = clients.OrderByDescending(s => s.FirstName);
                    break;
                case SortState.DescriptionAsc:
                    clients = clients.OrderBy(s => s.LastName);
                    break;
                case SortState.DescriptionDesc:
                    clients = clients.OrderByDescending(s => s.LastName);
                    break;
                case SortState.CostAsc:
                    clients = clients.OrderBy(s => s.Address);
                    break;
                case SortState.CostDesc:
                    clients = clients.OrderByDescending(s => s.Address);
                    break;
                default:
                    clients = clients.OrderBy(s => s.ClientId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchFirstName))
            {
                clients = clients.Where(s => s.FirstName.Contains(searchFirstName));
            }

            if (!string.IsNullOrEmpty(searchLastName))
            {
                clients = clients.Where(s => s.LastName.Contains(searchLastName));
            }

            return clients;
        }
    }
}
