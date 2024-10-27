using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedAdditionalServicesService : ICachedAdditionalServicesService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedAdditionalServicesService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddAdditionalServices(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<AdditionalService> additionalServices = _dbContext.AdditionalServices.Take(rowsNumber).ToList();
            if (additionalServices != null)
            {
                _memoryCache.Set(cacheKey, additionalServices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240) 
                });
            }
        }

        public IEnumerable<AdditionalService> GetAdditionalServices(int rowsNumber = 20)
        {
            return _dbContext.AdditionalServices.Take(rowsNumber).ToList();
        }

        public IEnumerable<AdditionalService> GetAdditionalServices(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<AdditionalService> additionalServices;
            if (!_memoryCache.TryGetValue(cacheKey, out additionalServices))
            {
                additionalServices = _dbContext.AdditionalServices.Take(rowsNumber).ToList();
                if (additionalServices != null)
                {
                    _memoryCache.Set(cacheKey, additionalServices,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240))); 
                }
            }
            return additionalServices;
        }
    }
}
