using System;
using System.Collections.Generic;

namespace TaskManager.Web.Models;

public partial class Reminder
{
    public long Id { get; set; }

    public long TodoId { get; set; }

    public DateTime ReminderDate { get; set; }

    public bool IsSent { get; set; }

    public virtual Todo Todo { get; set; } = null!;
}
