using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        [Display(Name = "Дата заказа")]
        [DataType(DataType.Date)]
        public DateOnly? OrderDate { get; set; }

        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateOnly? StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }

        [Display(Name = "Клиент")]
        public int? ClientId { get; set; }

        [Display(Name = "Локация")]
        public int? LocationId { get; set; }

        [Display(Name = "Сотрудник")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Общая стоимость")]
        public decimal? TotalCost { get; set; }

        [Display(Name = "Оплачено")]
        public bool? Paid { get; set; }

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }

        public bool IsPaid
        {
            get => Paid ?? false; // Если Paid равно null, возвращаем false
            set => Paid = value; // Устанавливаем значение Paid
        }
    }
}
