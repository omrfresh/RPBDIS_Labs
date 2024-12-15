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
    public class AdTypesController : ControllerBase
    {
        private readonly AdvertisingDbContext _context;

        public AdTypesController(AdvertisingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка всех типов рекламы
        /// </summary>
        // GET api/adtypes
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<AdTypeViewModel>>> GetAdTypes()
        {
            var adTypes = await _context.AdTypes
                .Select(a => new AdTypeViewModel
                {
                    AdTypeId = a.AdTypeId,
                    Name = a.Name,
                    Description = a.Description
                })
                .ToListAsync();

            return adTypes;
        }

        /// <summary>
        /// Получение данных одного типа рекламы
        /// </summary>
        /// <remarks>
        /// Описание параметра
        /// </remarks>
        /// <param name="id">Код типа рекламы</param>
        /// <returns>JSON</returns>
        // GET api/adtypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdTypeViewModel>> GetAdType(int id)
        {
            var adType = await _context.AdTypes
                .Select(a => new AdTypeViewModel
                {
                    AdTypeId = a.AdTypeId,
                    Name = a.Name,
                    Description = a.Description
                })
                .FirstOrDefaultAsync(a => a.AdTypeId == id);

            if (adType == null)
            {
                return NotFound();
            }

            return adType;
        }

        /// <summary>
        /// Регистрация нового типа рекламы
        /// </summary>
        // POST api/adtypes
        [HttpPost]
        public async Task<ActionResult<AdTypeViewModel>> PostAdType([FromBody] AdTypeViewModel adType)
        {
            if (adType == null)
            {
                return BadRequest();
            }

            var newAdType = new AdType
            {
                Name = adType.Name,
                Description = adType.Description
            };

            _context.AdTypes.Add(newAdType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdType", new { id = newAdType.AdTypeId }, adType);
        }

        /// <summary>
        /// Обновление данных одного типа рекламы
        /// </summary>
        /// <remarks>
        /// Объект передается в теле запроса
        /// </remarks>
        /// <param name="adType">объект, определяющий тип рекламы</param>
        /// <returns>Статус</returns>
        // PUT api/adtypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdType(int id, [FromBody] AdTypeViewModel adType)
        {
            if (id != adType.AdTypeId)
            {
                return BadRequest();
            }

            var existingAdType = await _context.AdTypes.FindAsync(id);
            if (existingAdType == null)
            {
                return NotFound();
            }

            existingAdType.Name = adType.Name;
            existingAdType.Description = adType.Description;

            _context.Entry(existingAdType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdTypeExists(id))
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
        /// Удаление данных одного типа рекламы
        /// </summary>
        /// <remarks>
        /// Описание параметра
        /// </remarks>
        /// <param name="id">Код типа рекламы</param>
        /// <returns>Статус</returns>
        // DELETE api/adtypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdType(int id)
        {
            var adType = await _context.AdTypes.FindAsync(id);
            if (adType == null)
            {
                return NotFound();
            }

            _context.AdTypes.Remove(adType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdTypeExists(int id)
        {
            return _context.AdTypes.Any(e => e.AdTypeId == id);
        }

        /// <summary>
        /// Получение HTML-страницы для отображения типов рекламы
        /// </summary>
        /// <returns>HTML</returns>
        // GET api/adtypes/fetch_adtypes
        [HttpGet("fetch_adtypes")]
        public IActionResult FetchAdTypes()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fetch_adtypes.html"), "text/HTML");
        }

        /// <summary>
        /// Получение HTML-страницы для отображения типов рекламы
        /// </summary>
        /// <returns>HTML</returns>
        // GET api/adtypes/jq_adtypes
        [HttpGet("jq_adtypes")]
        public IActionResult JqAdTypes()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "jq_adtypes.html"), "text/HTML");
        }
    }
}
