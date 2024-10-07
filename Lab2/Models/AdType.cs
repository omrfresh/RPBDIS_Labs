using System;
using System.Collections.Generic;

namespace Lab2_RPBDIS;

public partial class AdType
{
    public int AdTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
