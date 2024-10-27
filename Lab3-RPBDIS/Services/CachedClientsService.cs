using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedClientsService : ICachedClientsService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedClientsService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddClients(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Client> clients = _dbContext.Clients.Take(rowsNumber).ToList();
            if (clients != null)
            {
                _memoryCache.Set(cacheKey, clients, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240) 
                });
            }
        }

        public IEnumerable<Client> GetClients(int rowsNumber = 20)
        {
            return _dbContext.Clients.Take(rowsNumber).ToList();
        }

        public IEnumerable<Client> GetClients(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Client> clients;
            if (!_memoryCache.TryGetValue(cacheKey, out clients))
            {
                clients = _dbContext.Clients.Take(rowsNumber).ToList();
                if (clients != null)
                {
                    _memoryCache.Set(cacheKey, clients,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240))); 
                }
            }
            return clients;
        }
    }
}
