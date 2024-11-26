using Lab4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lab4.ViewModels
{
    public class LocationsViewModel
    {
        public IEnumerable<Location> Locations { get; set; }

        // Свойство для фильтрации
        public LocationViewModel LocationViewModel { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        // Список отчетных годов
        public SelectList ListYears { get; set; }
    }
}
