using Lab4.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class OrderServiceViewModel
    {
        public int OrderServiceId { get; set; }

        [Display(Name = "Заказ")]
        public int OrderId { get; set; }

        [Display(Name = "Услуга")]
        public int ServiceId { get; set; } 

        [Display(Name = "Количество")]
        public int? Quantity { get; set; }

        [Display(Name = "Общая стоимость")]
        public decimal? TotalCost { get; set; }

        public List<Order> Orders { get; set; }
        public List<AdditionalService> AdditionalServices { get; set; }
        public SortViewModel SortViewModel { get; set; }
    }
}