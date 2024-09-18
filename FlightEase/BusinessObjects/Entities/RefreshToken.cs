using System;
using System.Collections.Generic;

namespace BusinessObjects.Entities;

public partial class RefreshToken
{
    public int TokenId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ExpireDate { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual User? User { get; set; }
}
