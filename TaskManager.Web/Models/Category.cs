using System;
using System.Collections.Generic;

namespace TaskManager.Web.Models;

public partial class Category
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
}
