using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class Membership
{
    public int MembershipId { get; set; }

    public string? Rank { get; set; }

    public double? Discount { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
