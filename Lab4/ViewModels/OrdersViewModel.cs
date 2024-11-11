using Lab4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Lab4.ViewModels
{
    public class OrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

        // Свойство для фильтрации
        public OrderViewModel OrderViewModel { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        // Список отчетных годов
        public SelectList ListYears { get; set; }
    }
}
