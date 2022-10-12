using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication11.Model;
using WebApplication11.Repository.Products;

namespace WebApplication11.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        [HttpPost]
        public  async Task<IActionResult> insertProductToDb([FromBody] Product product)
        {
            int rowsAffected = await _productRepository.insertProductToDbAsync(product.size,product.type, product.brand, product.colour, product.price, product.name, product.quantity);
            if (rowsAffected == 1)
            {
                return Created("Successful", product);                                                     // If rowsAffected is 1 return Created
            }
            else {
                return BadRequest();                                                                                // If not return BadRequest
            }
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> getAllProductsFromDbAsync() {
            var products = await _productRepository.getAllProductsFromDbAsync();
            return products;
        }
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductfromDb(int productId)
        {
            try
            {
                await _productRepository.DeleteProductFromDb(productId);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        // Update product's quantity
        [Route("{productId}/quantity")]
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateQuantityOfProduct(int productId, [FromBody] Product product)
        {
            try
            {
                await _productRepository.UpdateProductQuantityInDb(product.quantity, productId);
                return Ok();
            }
            catch { return BadRequest(); }
        }

        [Route("{productId}/price")]
        [HttpPut]
        public async Task<IActionResult> UpdatePriceOfProduct(int productId, [FromBody] Product product)
        {
            try
            {
                await _productRepository.UpdateProductPriceInDb(product.price, productId);
                return Ok();
            }
            catch { return BadRequest(); }
        }
        [HttpGet("{productId}")]
        public async Task<ProductReturnable> GetProduct(int productId)
        {
            try
            {
                return await _productRepository.getProductFromDbByIdAsync(productId);
            }
            catch { return null; }
        }
    }
}
