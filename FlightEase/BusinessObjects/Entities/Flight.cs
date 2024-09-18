using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Flight
{
    public int FlightId { get; set; }

    public int? PilotId { get; set; }

    public int? FlightNumber { get; set; }

    public int? FlightRouteId { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public string? FlightStatus { get; set; }

    public int? EmptySeat { get; set; }

    public virtual FlightRoute? FlightRoute { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Pilot? Pilot { get; set; }

    public virtual ICollection<SeatFlight> SeatFlights { get; set; } = new List<SeatFlight>();

    public virtual ICollection<Pilot> Pilots { get; set; } = new List<Pilot>();
}
