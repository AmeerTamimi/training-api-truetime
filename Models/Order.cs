using System.ComponentModel.DataAnnotations;

namespace Training_API.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set;}

    public string? Status { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>(); // complex data

    public virtual List<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }
}
