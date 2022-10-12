using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Managers;
using WebApplication11.Model;
using WebApplication11.Repository;
using WebApplication11.Repository.Cart;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication11.Controllers.Cart
{
    [Route("api/User/{userId}/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IJWTAuthenticationManager _JWTAuthenticationManager;
        private readonly IUserRepository _userRepository;
        public CartController(ICartRepository cartRepository, IJWTAuthenticationManager jWTAuthenticationManager, IUserRepository userRepository) {
            _JWTAuthenticationManager = jWTAuthenticationManager;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }
        [HttpGet()]
        public async Task<IEnumerable<Product>> getAllProductsInCartFromDb(int userId) {                         
            var cartId = await _cartRepository.GetCartIdOfUserFromDb(userId);

            IEnumerable<Product> products = await _cartRepository.getAllProductsInCartFromDbAsync(cartId);
            return products;                                                                                    // Returning all the products
        }


        [HttpPost]
        public async Task<IActionResult> InsertProductToCartDb([FromBody] Product product) {                        // Getting the product from the request body
            var token = Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;                    // Validating the user first

            try {
                var userId = int.Parse(_JWTAuthenticationManager.ValidateToken(token));
                var cartId = await _cartRepository.GetCartIdOfUserFromDb(userId);
                if (cartId == 0) {
                await _cartRepository.InsertCartToDbByUserIdAsync(userId);
                cartId = await _cartRepository.GetCartIdOfUserFromDb(userId);
                }
                //Checking if cart has that item already or not
                IEnumerable<Product> productsInCart = await _cartRepository.getAllProductsInCartFromDbAsync(cartId);
                if (productsInCart.Any(x => x.productId == product.productId))
                {
                    return StatusCode(403);                 // returns resource already exists
                }
                else
                {
                    bool asd = productsInCart.Any(x => x.productId == product.productId);
                    int rowsAffected = await _cartRepository.InsertProductToCartDb(cartId, product.productId);   // Insterting the product into cart. Returning the number of rows affected
                    if (rowsAffected == 1)
                    {                                       // If thats 1, return Ok
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // Deleting products from cart
        [HttpDelete]
        public async Task<IActionResult> deleteProductsFromCartDb() {                                       
            var token = Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;                        // Validating the user first
            int userId = int.Parse(_JWTAuthenticationManager.ValidateToken(token));

            var numberOfProducts = await _cartRepository.getNumberOfProductsInCartFromDb(userId);           // Getting the number of products currently in cart
            int[] number = numberOfProducts.ToArray();

            var deleteCartTask = await _cartRepository.deleteProductsFromCartDbAsync(userId);                    // Deleting products from cart, returning the number of rows affected
            if (numberOfProducts.FirstOrDefault() == deleteCartTask) {                                      // If those 2 numbers are the same, return Ok
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> deleteSpecificProductFromCartDb(int productId, int userId) {
            //int productId = int.Parse(HttpContext.Request.Query["productId"]);

            var deletedProductFromCart = await _cartRepository.deleteSpecificProductFromCartDbAsync(userId, productId);  // deleting a product from cart
            if (deletedProductFromCart != 0) {
                return Ok();
            }
            return BadRequest();
        }
        // Add +1 to products quantity in cart
        [HttpPost("add")]
        public async Task<IActionResult> AddQuantityOfProductInCart(int userId, [FromBody]Product product) {
            int cartId = await _cartRepository.GetCartIdOfUserFromDb(userId);

            int rowsAffected = await _cartRepository.AddQuantityOfProductInCart(product.productId, cartId);
            if (rowsAffected == 1) {
                return Ok();
            }
            return BadRequest();
        }
        // Remove 1 from quantity of product in cart
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveQuantityOfProductInCart(int userId, [FromBody]Product product) {
            int cartId = await _cartRepository.GetCartIdOfUserFromDb(userId);

            int rowsAffected = await _cartRepository.RemoveQuantityOfProductInCart(product.productId, cartId);
            if (rowsAffected == 1)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
