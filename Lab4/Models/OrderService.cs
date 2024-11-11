namespace Lab4.Models
{
    using System;
    using System.Collections.Generic;

    public partial class OrderService
    {
        public int OrderServiceId { get; set; }

        public int OrderId { get; set; }

        public int ServiceId { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalCost { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual AdditionalService Service { get; set; } = null!;
    }
}
