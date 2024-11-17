using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class RentalVehicle
{
    public int VehicleId { get; set; }

    public string? VehicleType { get; set; }

    public string? VehicleModel { get; set; }

    public string? VehicleDescription { get; set; }

    public decimal? RentalPricePerDay { get; set; }

    public string? AvailabilityStatus { get; set; }

    public int? AddedBy { get; set; }

    public DateTime? DateAdded { get; set; }

    public virtual User? AddedByNavigation { get; set; }

    public virtual ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();
}
