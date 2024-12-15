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
    public class LocationsController : ControllerBase
    {
        private readonly AdvertisingDbContext _context;

        public LocationsController(AdvertisingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получение списка всех локаций
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationViewModel>>> GetLocations()
        {
            var locations = await _context.Locations
                .Include(l => l.AdType)
                .Select(l => new LocationViewModel
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    LocationDescription = l.LocationDescription,
                    AdTypeId = l.AdTypeId,
                    AdDescription = l.AdDescription,
                    Cost = l.Cost,
                    AdTypeName = l.AdType != null ? l.AdType.Name : null
                })
                .ToListAsync();

            return Ok(locations);
        }

        /// <summary>
        /// Получение данных одной локации
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationViewModel>> GetLocation(int id)
        {
            var location = await _context.Locations
                .Include(l => l.AdType)
                .Select(l => new LocationViewModel
                {
                    LocationId = l.LocationId,
                    Name = l.Name,
                    LocationDescription = l.LocationDescription,
                    AdTypeId = l.AdTypeId,
                    AdDescription = l.AdDescription,
                    Cost = l.Cost,
                    AdTypeName = l.AdType != null ? l.AdType.Name : null
                })
                .FirstOrDefaultAsync(l => l.LocationId == id);

            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        /// <summary>
        /// Регистрация новой локации
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LocationViewModel>> PostLocation([FromBody] LocationViewModel location)
        {
            if (location == null)
            {
                return BadRequest("Location data is null.");
            }

            var newLocation = new Location
            {
                Name = location.Name,
                LocationDescription = location.LocationDescription,
                AdTypeId = location.AdTypeId,
                AdDescription = location.AdDescription,
                Cost = location.Cost
            };

            _context.Locations.Add(newLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = newLocation.LocationId }, location);
        }

        /// <summary>
        /// Обновление данных одной локации
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, [FromBody] LocationViewModel location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращаем все ошибки валидации
            }

            if (id != location.LocationId)
            {
                return BadRequest("Location ID mismatch.");
            }

            var existingLocation = await _context.Locations.FindAsync(id);
            if (existingLocation == null)
            {
                return NotFound();
            }

            // Обновление полей существующей локации
            existingLocation.Name = location.Name;
            existingLocation.LocationDescription = location.LocationDescription;
            existingLocation.AdTypeId = location.AdTypeId;
            existingLocation.AdDescription = location.AdDescription;
            existingLocation.Cost = location.Cost;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Возвращаем только статус 204
        }

        /// <summary>
        /// Удаление данных одной локации
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Получение списка всех типов рекламы
        /// </summary>
        [HttpGet("adtypes")]
        public async Task<ActionResult<IEnumerable<AdTypeViewModel>>> GetAdTypes()
        {
            var adTypes = await _context.AdTypes
                .Select(at => new AdTypeViewModel
                {
                    AdTypeId = at.AdTypeId,
                    Name = at.Name
                })
                .ToListAsync();

            return Ok(adTypes);
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}