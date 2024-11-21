using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserImage { get; set; }

    public string? Password { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public byte[]? PasswordHash { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Description { get; set; }

    public string? Profession { get; set; }

    public string? ProfessionDescription { get; set; }

    public int? YearsOfExperience { get; set; }

    public bool? HasWarranty { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<CartsProduct> CartsProducts { get; set; } = new List<CartsProduct>();

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RentalRequest> RentalRequests { get; set; } = new List<RentalRequest>();

    public virtual ICollection<RentalVehicle> RentalVehicles { get; set; } = new List<RentalVehicle>();

    public virtual ICollection<UserService> UserServices { get; set; } = new List<UserService>();
}
