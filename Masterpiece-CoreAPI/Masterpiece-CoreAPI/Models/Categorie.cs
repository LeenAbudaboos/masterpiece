using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class Categorie
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public string? CategoryImg { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<UserService> UserServices { get; set; } = new List<UserService>();
}
