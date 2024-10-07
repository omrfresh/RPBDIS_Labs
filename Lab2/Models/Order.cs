using System;
using System.Collections.Generic;

namespace Lab2_RPBDIS;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? ClientId { get; set; }

    public int? LocationId { get; set; }

    public int? EmployeeId { get; set; }

    public decimal? TotalCost { get; set; }

    public bool? Paid { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<OrderService> OrderServices { get; set; } = new List<OrderService>();
}
