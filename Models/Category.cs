using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Training_API.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public virtual List<Product> Products { get; set; } = new List<Product>();
}
