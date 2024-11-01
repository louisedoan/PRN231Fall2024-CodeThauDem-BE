﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Plane
{
    public int PlaneId { get; set; }

    public string? PlaneCode { get; set; }

    public int? TotalSeats { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
