using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductImg { get; set; }

    public string? Descriptions { get; set; }

    public int? Price { get; set; }

    public int? PriceDetels { get; set; }

    public int? Quantity { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Categorie? Category { get; set; }
}
