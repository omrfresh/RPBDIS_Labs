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

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
