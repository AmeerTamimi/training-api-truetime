using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Training_API.Models;

public partial class User
{

    [Key]
    public int UserId { get; set; }
    [Required]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required]

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? Phone { get; set; }

    public bool? IsAdmin { get; set; }

    public DateTime? CreatedAt { get; set; }

    

    public virtual List<Address>? AddressList { get; set; } = new List<Address>(); // complex 

    public virtual List<Order>? Orders { get; set; } = new List<Order>(); // complex
}
