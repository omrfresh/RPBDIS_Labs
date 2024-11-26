using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class ClientViewModel
    {
        public int ClientId { get; set; }

        [Display(Name = "Имя")]
        public string? FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
