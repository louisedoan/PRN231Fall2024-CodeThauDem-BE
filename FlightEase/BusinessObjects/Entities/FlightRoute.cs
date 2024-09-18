using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class FlightRoute
{
    public int FlightRouteId { get; set; }

    public string? Location { get; set; }

    public string? Duration { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
