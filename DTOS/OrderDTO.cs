namespace Training_API.DTOS
{
    public class OrderDTO
    {
        public int OrderId { get; set; }

        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public List<OrderDetailsDTO>? orderItems { get; set; }

        public List<PaymentDTO>? Payments { get; set; }
    }
}
