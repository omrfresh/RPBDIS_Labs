//using Lab4.Data;
//using Lab4.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;

//namespace Lab4.Controllers
//{
//    public class EmployeesController : Controller
//    {
//        private readonly AdvertisingDbContext _context;

//        public EmployeesController(AdvertisingDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No)
//        {
//            int pageSize = 10;
//            var employees = _context.Employees.AsQueryable();

//            switch (sortOrder)
//            {
//                case SortState.NameAsc:
//                    employees = employees.OrderBy(s => s.FirstName);
//                    break;
//                case SortState.NameDesc:
//                    employees = employees.OrderByDescending(s => s.FirstName);
//                    break;
//                case SortState.DescriptionAsc:
//                    employees = employees.OrderBy(s => s.LastName);
//                    break;
//                case SortState.DescriptionDesc:
//                    employees = employees.OrderByDescending(s => s.LastName);
//                    break;
//                case SortState.CostAsc:
//                    employees = employees.OrderBy(s => s.Position);
//                    break;
//                case SortState.CostDesc:
//                    employees = employees.OrderByDescending(s => s.Position);
//                    break;
//                default:
//                    employees = employees.OrderBy(s => s.EmployeeId);
//                    break;
//            }

//            var count = employees.Count();
//            var items = employees.Skip((page - 1) * pageSize).Take(pageSize).ToList();

//            var viewModel = new EmployeesViewModel
//            {
//                Employees = items,
//                PageViewModel = new PageViewModel(count, page, pageSize),
//                EmployeeViewModel = new EmployeeViewModel
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
    public class EmployeesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public EmployeesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.No, string searchFirstName = "", string searchLastName = "")
        {
            IQueryable<Employee> employees = _context.Employees.AsQueryable();

            // Сортировка и фильтрация данных
            employees = Sort_Search(employees, sortOrder, searchFirstName, searchLastName);

            // Разбиение на страницы
            var count = employees.Count();
            employees = employees.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            var viewModel = new EmployeesViewModel
            {
                Employees = employees,
                PageViewModel = new PageViewModel(count, page, pageSize),
                EmployeeViewModel = new EmployeeViewModel
                {
                    SortViewModel = new SortViewModel(sortOrder),
                    FirstName = searchFirstName,
                    LastName = searchLastName
                }
            };

            return View(viewModel);
        }

        // POST: Employees
        [HttpPost]
        public IActionResult Index(EmployeeViewModel employeeViewModel, int page = 1)
        {
            if (employeeViewModel == null)
            {
                employeeViewModel = new EmployeeViewModel();
            }

            if (employeeViewModel.SortViewModel == null)
            {
                employeeViewModel.SortViewModel = new SortViewModel(SortState.No);
            }

            return RedirectToAction("Index", new { page, sortOrder = employeeViewModel.SortViewModel.CurrentState, searchFirstName = employeeViewModel.FirstName, searchLastName = employeeViewModel.LastName });
        }

        // Сортировка и фильтрация данных
        private static IQueryable<Employee> Sort_Search(IQueryable<Employee> employees, SortState sortOrder, string searchFirstName, string searchLastName)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    employees = employees.OrderBy(s => s.FirstName);
                    break;
                case SortState.NameDesc:
                    employees = employees.OrderByDescending(s => s.FirstName);
                    break;
                case SortState.DescriptionAsc:
                    employees = employees.OrderBy(s => s.LastName);
                    break;
                case SortState.DescriptionDesc:
                    employees = employees.OrderByDescending(s => s.LastName);
                    break;
                case SortState.CostAsc:
                    employees = employees.OrderBy(s => s.Position);
                    break;
                case SortState.CostDesc:
                    employees = employees.OrderByDescending(s => s.Position);
                    break;
                default:
                    employees = employees.OrderBy(s => s.EmployeeId);
                    break;
            }

            if (!string.IsNullOrEmpty(searchFirstName))
            {
                employees = employees.Where(s => s.FirstName.Contains(searchFirstName));
            }

            if (!string.IsNullOrEmpty(searchLastName))
            {
                employees = employees.Where(s => s.LastName.Contains(searchLastName));
            }

            return employees;
        }
    }
}
