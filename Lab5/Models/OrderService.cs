namespace Lab4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class OrderService
    {
        public int OrderServiceId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int? Quantity { get; set; }

        [Required]
        public decimal? TotalCost { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual AdditionalService Service { get; set; } = null!;
    }
}
