using Lab4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lab4.ViewModels
{
    public class EmployeesViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        // Свойство для фильтрации
        public EmployeeViewModel EmployeeViewModel { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        // Список отчетных годов
        public SelectList ListYears { get; set; }
    }
}
