using Lab6.Data;
using Lab6.Models;
using Lab6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AdvertisingDbContext _context;

        public ClientsController(AdvertisingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка всех клиентов
        /// </summary>
        // GET api/clients
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<ClientViewModel>>> GetClients()
        {
            var clients = await _context.Clients
                .Select(c => new ClientViewModel
                {
                    ClientId = c.ClientId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber
                })
                .ToListAsync();

            return clients;
        }

        /// <summary>
        /// Получение данных одного клиента
        /// </summary>
        /// <remarks>
        /// Описание параметра
        /// </remarks>
        /// <param name="id">Код клиента</param>
        /// <returns>JSON</returns>
        // GET api/clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientViewModel>> GetClient(int id)
        {
            var client = await _context.Clients
                .Select(c => new ClientViewModel
                {
                    ClientId = c.ClientId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber
                })
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        /// <summary>
        /// Регистрация нового клиента
        /// </summary>
        // POST api/clients
        [HttpPost]
        public async Task<ActionResult<ClientViewModel>> PostClient([FromBody] ClientViewModel client)
        {
            if (client == null)
            {
                return BadRequest();
            }

            var newClient = new Client
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Address = client.Address,
                PhoneNumber = client.PhoneNumber
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = newClient.ClientId }, client);
        }

        /// <summary>
        /// Обновление данных одного клиента
        /// </summary>
        /// <remarks>
        /// Объект передается в теле запроса
        /// </remarks>
        /// <param name="client">объект, определяющий клиента</param>
        /// <returns>Статус</returns>
        // PUT api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, [FromBody] ClientViewModel client)
        {
            if (id != client.ClientId)
            {
                return BadRequest();
            }

            var existingClient = await _context.Clients.FindAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.FirstName = client.FirstName;
            existingClient.LastName = client.LastName;
            existingClient.Address = client.Address;
            existingClient.PhoneNumber = client.PhoneNumber;

            _context.Entry(existingClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Удаление данных одного клиента
        /// </summary>
        /// <remarks>
        /// Описание параметра
        /// </remarks>
        /// <param name="id">Код клиента</param>
        /// <returns>Статус</returns>
        // DELETE api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }

        /// <summary>
        /// Получение HTML-страницы для отображения клиентов
        /// </summary>
        /// <returns>HTML</returns>
        // GET api/clients/fetch_clients
        [HttpGet("fetch_clients")]
        public IActionResult FetchClients()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fetch_clients.html"), "text/HTML");
        }

        /// <summary>
        /// Получение HTML-страницы для отображения клиентов
        /// </summary>
        /// <returns>HTML</returns>
        // GET api/clients/jq_clients
        [HttpGet("jq_clients")]
        public IActionResult JqClients()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jq_clients.html"), "text/HTML");
        }
    }
}
