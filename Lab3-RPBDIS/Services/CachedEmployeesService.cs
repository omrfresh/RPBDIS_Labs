using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services
{
    public class CachedEmployeesService : ICachedEmployeesService
    {
        private readonly AdvertisingAgencyDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedEmployeesService(AdvertisingAgencyDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public void AddEmployees(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> employees = _dbContext.Employees.Take(rowsNumber).ToList();
            if (employees != null)
            {
                _memoryCache.Set(cacheKey, employees, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(2 * 18 + 240)
                });
            }
        }

        public IEnumerable<Employee> GetEmployees(int rowsNumber = 20)
        {
            return _dbContext.Employees.Take(rowsNumber).ToList();
        }

        public IEnumerable<Employee> GetEmployees(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> employees;
            if (!_memoryCache.TryGetValue(cacheKey, out employees))
            {
                employees = _dbContext.Employees.Take(rowsNumber).ToList();
                if (employees != null)
                {
                    _memoryCache.Set(cacheKey, employees,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 18 + 240))); 
                }
            }
            return employees;
        }
    }
}
