using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public string TransportMethod { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? LocationLink { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
