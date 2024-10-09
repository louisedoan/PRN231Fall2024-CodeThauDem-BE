using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Refund
{
    public int RefundId { get; set; }

    public int? OrderDetailId { get; set; }

    public DateTime? RefundDate { get; set; }

    public double? Amount { get; set; }

    public string? Status { get; set; }

    public virtual OrderDetail? OrderDetail { get; set; }
}
