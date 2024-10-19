using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class PartnerProduct
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public string? ProductImage { get; set; }

    public string? PartnerCompanyName { get; set; }

    public string? PartnerName { get; set; }

    public string? PartnerEmail { get; set; }

    public string? PartnerPhone { get; set; }

    public DateTime? DateSubmitted { get; set; }

    public int? ProductPrice { get; set; }
}
