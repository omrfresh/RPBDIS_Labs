using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedAdTypesService : ICachedAdTypesService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedAdTypesService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddAdTypes(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<AdType> adTypes = _dbContext.AdTypes.Take(rowsNumber).ToList();
            if (adTypes != null)
            {
                _memoryCache.Set(cacheKey, adTypes, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240) 
                });
            }
        }

        public IEnumerable<AdType> GetAdTypes(int rowsNumber = 20)
        {
            return _dbContext.AdTypes.Take(rowsNumber).ToList();
        }

        public IEnumerable<AdType> GetAdTypes(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<AdType> adTypes;
            if (!_memoryCache.TryGetValue(cacheKey, out adTypes))
            {
                adTypes = _dbContext.AdTypes.Take(rowsNumber).ToList();
                if (adTypes != null)
                {
                    _memoryCache.Set(cacheKey, adTypes,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240)));
                }
            }
            return adTypes;
        }
    }
}
