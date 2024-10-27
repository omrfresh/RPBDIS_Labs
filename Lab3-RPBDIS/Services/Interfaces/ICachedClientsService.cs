using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedClientsService
    {
        IEnumerable<Client> GetClients(int rowsNumber = 20);
        void AddClients(string cacheKey, int rowsNumber = 20);
        IEnumerable<Client> GetClients(string cacheKey, int rowsNumber = 20);
    }
}
