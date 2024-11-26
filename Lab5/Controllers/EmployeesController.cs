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
    public class EmployeesController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

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
                Employees = employees.ToList(),
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

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Position")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Position")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                if (_context.Orders.Any(o => o.EmployeeId == id))
                {
                    return View("CannotDelete", "Невозможно удалить этого сотрудника, так как он используется в других записях.");
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Сортировка и фильтрация данных
        private static IQueryable<Employee> Sort_Search(IQueryable<Employee> employees, SortState sortOrder, string searchFirstName, string searchLastName)
        {
            if (!string.IsNullOrEmpty(searchFirstName))
            {
                employees = employees.Where(s => s.FirstName.Contains(searchFirstName));
            }

            if (!string.IsNullOrEmpty(searchLastName))
            {
                employees = employees.Where(s => s.LastName.Contains(searchLastName));
            }

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

            return employees;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
