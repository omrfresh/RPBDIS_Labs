using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedLocationsService
    {
        IEnumerable<Location> GetLocations(int rowsNumber = 20);
        void AddLocations(string cacheKey, int rowsNumber = 20);
        IEnumerable<Location> GetLocations(string cacheKey, int rowsNumber = 20);
    }
}
