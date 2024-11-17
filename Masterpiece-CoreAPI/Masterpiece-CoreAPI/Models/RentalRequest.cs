using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class RentalRequest
{
    public int RequestId { get; set; }

    public int? VehicleId { get; set; }

    public int? UserId { get; set; }

    public DateTime? RequestDate { get; set; }

    public DateTime? RentalStartDate { get; set; }

    public DateTime? RentalEndDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? RequestStatus { get; set; }

    public virtual User? User { get; set; }

    public virtual RentalVehicle? Vehicle { get; set; }
}
