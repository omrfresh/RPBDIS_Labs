using System;
using System.Collections.Generic;

namespace Lab2_RPBDIS;

public partial class Client
{
    public int ClientId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
