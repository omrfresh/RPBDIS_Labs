using Lab4.Models;

namespace Lab4.Models
{
    public partial class AdditionalService
    {
        public int AdditionalServiceId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Cost { get; set; }

        public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
    }
}
