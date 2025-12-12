using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training_API.Data;
using Training_API.DTOS;
using Training_API.Models;

namespace Training_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyAppContext _db;

        public CategoryController(MyAppContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _db.Categories
                .Include(x => x.Products)
                .ToListAsync();

            var categoriesDTO = new List<CategoryDTO>();

            foreach (var category in categories)
            {
                var categoryDTO = new CategoryDTO()
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                    Products = new List<ProductDTO>()
                };

                if (category.Products != null && category.Products.Any())
                {
                    foreach (var product in category.Products)
                    {
                        var productDTO = new ProductDTO()
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            Description = product.Description,
                            Price = product.Price,
                            StockQuantity = product.StockQuantity,
                            CategoryId = product.CategoryId,
                            ImageUrl = product.ImageUrl,
                            IsActive = product.IsActive
                        };
                        categoryDTO.Products.Add(productDTO);
                    }
                }

                categoriesDTO.Add(categoryDTO);
            }

            return Ok(categoriesDTO);
        }

        [HttpGet("{CategoryID}")]
        public async Task<IActionResult> GetCategory(int CategoryID)
        {
            var category = await _db.Categories
                .Include(x => x.Products)
                .SingleOrDefaultAsync(x => x.CategoryId == CategoryID);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDTO = new CategoryDTO()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description,
                Products = new List<ProductDTO>()
            };

            if (category.Products != null && category.Products.Any())
            {
                foreach (var product in category.Products)
                {
                    var productDTO = new ProductDTO()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
                        CategoryId = product.CategoryId,
                        ImageUrl = product.ImageUrl,
                        IsActive = product.IsActive
                    };
                    categoryDTO.Products.Add(productDTO);
                }
            }

            return Ok(categoryDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCategory([FromBody] CategoryDTO newCategoryDTO)
        {
            if (newCategoryDTO == null)
            {
                return BadRequest();
            }

            var category = new Category()
            {
                CategoryName = newCategoryDTO.CategoryName,
                Description = newCategoryDTO.Description,
                Products = new List<Product>()
            };

            if (newCategoryDTO.Products != null && newCategoryDTO.Products.Any())
            {
                foreach (var productDTO in newCategoryDTO.Products)
                {
                    var product = new Product()
                    {
                        ProductName = productDTO.ProductName,
                        Description = productDTO.Description,
                        Price = productDTO.Price,
                        StockQuantity = productDTO.StockQuantity,
                        ImageUrl = productDTO.ImageUrl,
                        IsActive = productDTO.IsActive
                    };
                    category.Products.Add(product);
                }
            }

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return Ok(newCategoryDTO);
        }

        [HttpPut("{CategoryID}")]
        public async Task<IActionResult> UpdateCategory(int CategoryID, [FromBody] CategoryDTO updatedCategoryDTO)
        {
            var categoryToUpdate = await _db.Categories
                .Include(x => x.Products)
                .SingleOrDefaultAsync(x => x.CategoryId == CategoryID);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            categoryToUpdate.CategoryName = updatedCategoryDTO.CategoryName;
            categoryToUpdate.Description = updatedCategoryDTO.Description;

            if (updatedCategoryDTO.Products != null)
            {
                categoryToUpdate.Products.Clear();

                foreach (var productDTO in updatedCategoryDTO.Products)
                {
                    var product = new Product()
                    {
                        ProductName = productDTO.ProductName,
                        Description = productDTO.Description,
                        Price = productDTO.Price,
                        StockQuantity = productDTO.StockQuantity,
                        CategoryId = CategoryID,
                        ImageUrl = productDTO.ImageUrl,
                        IsActive = productDTO.IsActive
                    };
                    categoryToUpdate.Products.Add(product);
                }
            }

            await _db.SaveChangesAsync();
            return Ok(updatedCategoryDTO);
        }

        [HttpDelete("{CategoryID}")]
        public async Task<IActionResult> DeleteCategory(int CategoryID)
        {
            var categoryToDelete = await _db.Categories
                .Include(x => x.Products)
                .SingleOrDefaultAsync(x => x.CategoryId == CategoryID);

            if (categoryToDelete == null)
            {
                return NotFound();
            }

            if (categoryToDelete.Products != null && categoryToDelete.Products.Any())
            {
                _db.Products.RemoveRange(categoryToDelete.Products);
            }

            _db.Categories.Remove(categoryToDelete);
            await _db.SaveChangesAsync();

            return Ok($"Category with ID {CategoryID} has been deleted!");
        }
    }
}