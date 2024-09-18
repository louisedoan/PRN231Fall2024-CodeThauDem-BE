using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Pilot
{
    public int PilotId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightsNavigation { get; set; } = new List<Flight>();
}
