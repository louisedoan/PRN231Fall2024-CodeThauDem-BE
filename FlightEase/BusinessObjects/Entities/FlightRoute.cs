using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class FlightRoute
{
    public int FlightRouteId { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Flight> FlightArrivalLocationNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightDepartureLocationNavigations { get; set; } = new List<Flight>();
}
