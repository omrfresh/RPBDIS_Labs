using Lab4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lab4.ViewModels
{
    public class ClientsViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        // Свойство для фильтрации
        public ClientViewModel ClientViewModel { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        // Список отчетных годов
        public SelectList ListYears { get; set; }
    }
}
