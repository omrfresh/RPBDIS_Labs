using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_RPBDIS.Services.Interfaces
{
    public interface ICachedEmployeesService
    {
        IEnumerable<Employee> GetEmployees(int rowsNumber = 20);
        void AddEmployees(string cacheKey, int rowsNumber = 20);
        IEnumerable<Employee> GetEmployees(string cacheKey, int rowsNumber = 20);
    }
}
