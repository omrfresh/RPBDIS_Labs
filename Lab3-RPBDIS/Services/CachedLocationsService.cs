using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedLocationsService : ICachedLocationsService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedLocationsService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddLocations(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Location> locations = _dbContext.Locations.Take(rowsNumber).ToList();
            if (locations != null)
            {
                _memoryCache.Set(cacheKey, locations, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240) 
                });
            }
        }

        public IEnumerable<Location> GetLocations(int rowsNumber = 20)
        {
            return _dbContext.Locations.Take(rowsNumber).ToList();
        }

        public IEnumerable<Location> GetLocations(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Location> locations;
            if (!_memoryCache.TryGetValue(cacheKey, out locations))
            {
                locations = _dbContext.Locations.Take(rowsNumber).ToList();
                if (locations != null)
                {
                    _memoryCache.Set(cacheKey, locations,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240))); 
                }
            }
            return locations;
        }
    }
}
