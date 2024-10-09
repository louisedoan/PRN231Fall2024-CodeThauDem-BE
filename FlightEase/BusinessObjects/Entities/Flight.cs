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

    public virtual FlightRoute? ArrivalLocationNavigation { get; set; }

    public virtual FlightRoute? DepartureLocationNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
