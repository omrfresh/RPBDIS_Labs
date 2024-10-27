using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedAdditionalServicesService
    {
        IEnumerable<AdditionalService> GetAdditionalServices(int rowsNumber = 20);
        void AddAdditionalServices(string cacheKey, int rowsNumber = 20);
        IEnumerable<AdditionalService> GetAdditionalServices(string cacheKey, int rowsNumber = 20);
    }
}
