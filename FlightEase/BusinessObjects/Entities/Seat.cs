using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? PlaneId { get; set; }

    public int? SeatNumer { get; set; }

    public string? Class { get; set; }

    public string? Status { get; set; }

    public double? Price { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Plane? Plane { get; set; }
}
