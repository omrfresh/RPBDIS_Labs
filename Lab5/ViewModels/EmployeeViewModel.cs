using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Имя")]
        public string? FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Должность")]
        public string? Position { get; set; }

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
