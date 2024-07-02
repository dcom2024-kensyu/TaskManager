using System;
using System.Collections.Generic;

namespace TaskManager.Web.Models;

public partial class Comment
{
    public long Id { get; set; }

    public long TodoId { get; set; }

    public long UserId { get; set; }

    public string Comment1 { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public virtual Todo Todo { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
