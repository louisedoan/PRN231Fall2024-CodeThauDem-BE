using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? Status { get; set; }

    public double? Amount { get; set; }

    public virtual Order? Order { get; set; }
}
