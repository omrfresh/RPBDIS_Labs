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
    [Authorize] // Все действия требуют авторизации, кроме публичных
    public class ClientsController : Controller
    {
        private readonly AdvertisingDbContext _context;
        private readonly int pageSize = 10; // количество элементов на странице

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

            var viewModel = new ClientsViewModel
            {
                Clients = clients.ToList(),
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

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Address,PhoneNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,FirstName,LastName,Address,PhoneNumber")] Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Доступ только для администраторов
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                if (_context.Orders.Any(o => o.ClientId == id))
                {
                    return View("CannotDelete", "Невозможно удалить этого клиента, так как он используется в других записях.");
                }

                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Сортировка и фильтрация данных
        private static IQueryable<Client> Sort_Search(IQueryable<Client> clients, SortState sortOrder, string searchFirstName, string searchLastName)
        {
            if (!string.IsNullOrEmpty(searchFirstName))
            {
                clients = clients.Where(s => s.FirstName.Contains(searchFirstName));
            }

            if (!string.IsNullOrEmpty(searchLastName))
            {
                clients = clients.Where(s => s.LastName.Contains(searchLastName));
            }

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

            return clients;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}
