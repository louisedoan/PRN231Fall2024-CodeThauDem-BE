using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public string? Name { get; set; }

    public DateTime? DoB { get; set; }

    public string? Nationality { get; set; }

    public string? Email { get; set; }

    public int? FlightId { get; set; }

    public int? SeatId { get; set; }

    public string? Status { get; set; }

    public double? TotalAmount { get; set; }

    public virtual Flight? Flight { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();

    public virtual Seat? Seat { get; set; }
}
