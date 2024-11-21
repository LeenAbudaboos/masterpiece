using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? CartId { get; set; }

    public int? UserId { get; set; }

    public virtual CartsProduct? Cart { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
