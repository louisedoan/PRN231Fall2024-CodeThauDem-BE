using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Gender { get; set; }

    public string? Nationality { get; set; }

    public string? Address { get; set; }

    public string? Fullname { get; set; }

    public DateTime? Dob { get; set; }

    public string? Role { get; set; }

    public int? MembershipId { get; set; }

    public string? Status { get; set; }

    public virtual Membership? Membership { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
