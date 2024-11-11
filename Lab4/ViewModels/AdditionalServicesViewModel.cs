using Lab4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lab4.ViewModels
{
    public class AdditionalServicesViewModel
    {
        public IEnumerable<AdditionalService> AdditionalServices { get; set; }

        // Свойство для фильтрации
        public AdditionalServiceViewModel AdditionalServiceViewModel { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        // Список отчетных годов
        public SelectList ListYears { get; set; }
    }
}
