using System;
using System.Collections.Generic;

namespace TaskManager.Web.Models;

public partial class UserRole
{
    public long Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
