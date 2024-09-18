using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? SeatNumber { get; set; }

    public string? Class { get; set; }

    public string? Status { get; set; }

    public int? PlaneId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Plane? Plane { get; set; }

    public virtual ICollection<SeatFlight> SeatFlights { get; set; } = new List<SeatFlight>();
}
