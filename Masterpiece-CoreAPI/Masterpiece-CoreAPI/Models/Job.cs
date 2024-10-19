using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class Job
{
    public int JobId { get; set; }

    public string? JobTitle { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? Salary { get; set; }

    public int? CategoryId { get; set; }

    public string? JobImg { get; set; }

    public string? PostedBy { get; set; }

    public virtual Categorie? Category { get; set; }

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}
