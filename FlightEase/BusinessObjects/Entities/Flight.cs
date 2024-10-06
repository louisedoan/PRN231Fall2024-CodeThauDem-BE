using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Flight
{
    public int FlightId { get; set; }

    public int? FlightNumber { get; set; }

    public int? PlaneId { get; set; }

    public int? DepartureLocation { get; set; }

    public DateTime? DepartureTime { get; set; }

    public int? ArrivalLocation { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public string? FlightStatus { get; set; }

    public int? EmptySeat { get; set; }

    public virtual FlightRoute? ArrivalLocationNavigation { get; set; }

    public virtual FlightRoute? DepartureLocationNavigation { get; set; }

    public virtual Plane? Plane { get; set; }

    public virtual ICollection<SeatFlight> SeatFlights { get; set; } = new List<SeatFlight>();
}
