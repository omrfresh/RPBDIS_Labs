using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedAdTypesService
    {
        IEnumerable<AdType> GetAdTypes(int rowsNumber = 20);
        void AddAdTypes(string cacheKey, int rowsNumber = 20);
        IEnumerable<AdType> GetAdTypes(string cacheKey, int rowsNumber = 20);
    }
}
