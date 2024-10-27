using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedOrderServicesService
    {
        IEnumerable<OrderService> GetOrderServices(int rowsNumber = 20);
        void AddOrderServices(string cacheKey, int rowsNumber = 20);
        IEnumerable<OrderService> GetOrderServices(string cacheKey, int rowsNumber = 20);
    }
}
