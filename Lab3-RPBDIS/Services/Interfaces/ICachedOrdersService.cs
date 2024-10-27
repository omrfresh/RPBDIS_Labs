using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedOrdersService
    {
        IEnumerable<Order> GetOrders(int rowsNumber = 20);
        void AddOrders(string cacheKey, int rowsNumber = 20);
        IEnumerable<Order> GetOrders(string cacheKey, int rowsNumber = 20);
    }
}
