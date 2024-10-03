using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class UserService
{
    public int ServiceId { get; set; }

    public string? ServiceTitle { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public int? CategoryId { get; set; }

    public int? PostedBy { get; set; }

    public DateTime? PostedAt { get; set; }

    public virtual Categorie? Category { get; set; }

    public virtual User? PostedByNavigation { get; set; }
}
