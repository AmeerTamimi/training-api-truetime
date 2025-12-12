using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Training_API.Models;

public partial class Address
{
    [Key]
    public int AddressId { get; set; }

    public int? UserId { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public virtual User? User { get; set; }
}
