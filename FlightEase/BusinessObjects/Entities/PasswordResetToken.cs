using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class PasswordResetToken
{
    public int TokenId { get; set; }

    public string? Token { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public bool IsUsed { get; set; }

    public virtual User User { get; set; } = null!;
}
