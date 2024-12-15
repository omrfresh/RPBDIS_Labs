using System.ComponentModel.DataAnnotations;

namespace Lab6.ViewModels
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

    }
}
