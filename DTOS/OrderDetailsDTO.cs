namespace Training_API.DTOS
{
    public class OrderDetailsDTO
    {
        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}
