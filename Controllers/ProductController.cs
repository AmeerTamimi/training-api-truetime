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
    public class ProductController : ControllerBase
    {
        private readonly MyAppContext _db;

        public ProductController(MyAppContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> getProducts()
        {
            return Ok(await _db.Products.ToArrayAsync());
        }

        [HttpGet("{productID}")]
        public async Task<IActionResult> getProducts(int productID)
        {
            var product = await _db.Products.SingleOrDefaultAsync(x => x.ProductId == productID);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewProduct([FromBody] ProductDTO newProduct)
        {
            if(newProduct == null)
            {
                return BadRequest();
            }

            Product product = new Product()
            {
                ProductName = newProduct.ProductName,
                Description = newProduct.Description,
                Price = newProduct.Price,
                StockQuantity = newProduct.StockQuantity,
                CategoryId = newProduct.CategoryId,
                ImageUrl = newProduct.ImageUrl,
                IsActive = newProduct.IsActive
            };
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return Ok(newProduct);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO updatedProduct)
        {
            if(updatedProduct == null)
            {
                return BadRequest();
            }
            var product = await _db.Products.SingleOrDefaultAsync(x => x.ProductId == updatedProduct.ProductId);

                product.ProductName = updatedProduct.ProductName;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.StockQuantity = updatedProduct.StockQuantity;
                product.CategoryId = updatedProduct.CategoryId;
                product.ImageUrl = updatedProduct.ImageUrl;
                product.IsActive = updatedProduct.IsActive;

            await _db.SaveChangesAsync();
            return Ok(updatedProduct);

        }


        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int productID)
        {
            var product = await _db.Products.SingleOrDefaultAsync(x => x.ProductId == productID);

            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok(product);
        }
    }
}
