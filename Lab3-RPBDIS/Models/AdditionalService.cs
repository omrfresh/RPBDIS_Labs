using System;
using System.Collections.Generic;

namespace Lab3_RPBDIS;

public partial class AdditionalService
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Cost { get; set; }

    public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
}
