using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class JobApplication
{
    public int ApplicationId { get; set; }

    public int? JobId { get; set; }

    public int? UserId { get; set; }

    public DateTime? AppliedAt { get; set; }

    public virtual Job? Job { get; set; }

    public virtual User? User { get; set; }
}
