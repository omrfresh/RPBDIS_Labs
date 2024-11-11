using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class LocationViewModel
    {
        public int LocationId { get; set; }

        [Display(Name = "Название")]
        public string? Name { get; set; }

        [Display(Name = "Описание")]
        public string? LocationDescription { get; set; }

        [Display(Name = "Тип рекламы")]
        public int? AdTypeId { get; set; }

        [Display(Name = "Описание рекламы")]
        public string? AdDescription { get; set; }

        [Display(Name = "Стоимость")]
        public decimal? Cost { get; set; }

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
