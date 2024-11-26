using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class AdTypeViewModel
    {
        public int AdTypeId { get; set; }

        [Display(Name = "Название")]
        public string? Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
