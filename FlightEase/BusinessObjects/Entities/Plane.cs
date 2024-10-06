using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Plane
{
    public int PlaneId { get; set; }

    public string? PlaneCode { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
