using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Training_API.Models;

public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? AmountPaid { get; set; }

    public bool? IsConfirmed { get; set; }

    public virtual Order? Order { get; set; }
}
