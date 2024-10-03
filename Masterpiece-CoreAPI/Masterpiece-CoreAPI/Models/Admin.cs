using System;
using System.Collections.Generic;

namespace Masterpiece_CoreAPI.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? AdminName { get; set; }

    public string? Email { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public byte[]? PasswordHash { get; set; }

    public string? Password { get; set; }
}
