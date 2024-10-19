using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class PartnerJob
{
    public int JobId { get; set; }

    public string JobTitle { get; set; } = null!;

    public string? JobDescription { get; set; }

    public int? Salary { get; set; }

    public string? JobLocation { get; set; }

    public string? PartnerName { get; set; }

    public string? PartnerEmail { get; set; }

    public string? PartnerPhone { get; set; }

    public DateTime? DateSubmitted { get; set; }
}
