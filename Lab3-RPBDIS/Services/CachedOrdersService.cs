using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedOrdersService : ICachedOrdersService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedOrdersService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddOrders(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Order> orders = _dbContext.Orders.Take(rowsNumber).ToList();
            if (orders != null)
            {
                _memoryCache.Set(cacheKey, orders, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240) 
                });
            }
        }

        public IEnumerable<Order> GetOrders(int rowsNumber = 20)
        {
            return _dbContext.Orders.Take(rowsNumber).ToList();
        }

        public IEnumerable<Order> GetOrders(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Order> orders;
            if (!_memoryCache.TryGetValue(cacheKey, out orders))
            {
                orders = _dbContext.Orders.Take(rowsNumber).ToList();
                if (orders != null)
                {
                    _memoryCache.Set(cacheKey, orders,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240))); 
                }
            }
            return orders;
        }
    }
}
