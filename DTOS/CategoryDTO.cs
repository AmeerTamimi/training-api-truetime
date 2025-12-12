using Training_API.Models;

namespace Training_API.DTOS
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }

        public List<ProductDTO>? Products { get; set; } = null!;
    }
}
