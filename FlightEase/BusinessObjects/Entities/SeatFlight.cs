using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class SeatFlight
{
    public int SeatId { get; set; }

    public int FlightId { get; set; }

    public string? Status { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;
}
