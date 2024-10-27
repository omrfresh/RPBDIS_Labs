using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedOrderServicesService : ICachedOrderServicesService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedOrderServicesService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddOrderServices(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<OrderService> orderServices = _dbContext.OrderServices.Take(rowsNumber).ToList();
            if (orderServices != null)
            {
                _memoryCache.Set(cacheKey, orderServices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240) 
                });
            }
        }

        public IEnumerable<OrderService> GetOrderServices(int rowsNumber = 20)
        {
            return _dbContext.OrderServices.Take(rowsNumber).ToList();
        }

        public IEnumerable<OrderService> GetOrderServices(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<OrderService> orderServices;
            if (!_memoryCache.TryGetValue(cacheKey, out orderServices))
            {
                orderServices = _dbContext.OrderServices.Take(rowsNumber).ToList();
                if (orderServices != null)
                {
                    _memoryCache.Set(cacheKey, orderServices,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240))); 
                }
            }
            return orderServices;
        }
    }
}
