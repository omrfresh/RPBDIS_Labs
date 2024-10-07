using System;
using System.Collections.Generic;

namespace Lab2_RPBDIS;

public partial class Location
{
    public int LocationId { get; set; }

    public string? Name { get; set; }

    public string? LocationDescription { get; set; }

    public int? AdTypeId { get; set; }

    public string? AdDescription { get; set; }

    public decimal? Cost { get; set; }

    public virtual AdType? AdType { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
