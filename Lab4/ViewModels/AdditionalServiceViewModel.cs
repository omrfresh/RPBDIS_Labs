using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class AdditionalServiceViewModel
    {
        public int AdditionalServiceId { get; set; }

        [Display(Name = "Название")]
        public string? Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Стоимость")]
        public decimal? Cost { get; set; }

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
