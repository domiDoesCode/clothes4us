using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Cart
{
    public class CartRepository : ICartRepository
    {
        private string _connectionString;
        private readonly IConfiguration _configuration;
        public CartRepository(IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");                      // Getting the connection string from appsettings.json
        }
        public async Task<int> InsertCartToDbByUserIdAsync(int userId) {
            int rowsAffected;
            string sqlCommand = "insert into Cart (userId, status) values (@userId, 'active')";
            using (var connection = new SqlConnection(_connectionString)) {
                return rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                {
                    userId = userId
                });
            }
        }

        public async Task<int> GetNumberOfCartOfUserFromDbAsync(int userId) {
            int numberOfCartOfUser;
            string sqlCommand = "select COUNT(cartId) from Cart where userId = @userId and status = 'active'";
            using (var connection = new SqlConnection(_connectionString))
            {
                return numberOfCartOfUser = await connection.QueryFirstAsync<int>(sqlCommand, new
                {
                    userId = userId
                });
            }
        }

        public async Task<int> InsertProductToCartDb(int cartId, int productId)
        {
            int rowsAffected;
            string sqlCommand = "insert into CartProducts (cartId, productId, quantity) values (@cartId, @productId, 1)";
            using (var connection = new SqlConnection(_connectionString)) {
                return rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                {
                    cartId = cartId,
                    productId = productId
                });
            }
        }
        // Getting cartId of a user
        public async Task<int> GetCartIdOfUserFromDb(int userId) {
            int cartId;
            string SqlCommand = "select cartId from Cart where userId=@userId and status = 'active'";
            using (var connection = new SqlConnection(_connectionString)) {
                return cartId = await connection.QueryFirstOrDefaultAsync<int>(SqlCommand, new
                {
                    userId = userId
                });
            }
        }
        // Getting all products from a user's cart
        public async Task<IEnumerable<Product>> getAllProductsInCartFromDbAsync(int cartId) {
            IEnumerable<Product> products;
            string sqlCommand = "select Products.productId,Products.size,Products.type,Products.brand,Products.colour,Products.price, Products.name, Products.quantity as stockQuantity, CartProducts.quantity from CartProducts Inner Join Products on Products.productId = CartProducts.productId where CartProducts.cartId = @cartId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return products = await connection.QueryAsync<Product>(sqlCommand, new
                {
                    cartId = cartId
                });
            }
        }
        // Add +1 to quantity
        public async Task<int> AddQuantityOfProductInCart(int productId, int cartId) {
            int rowsAffected;
            string sqlCommand = "update CartProducts set quantity = quantity + 1 where cartId = @cartId and productId = @productId";
            using (var connection = new SqlConnection(_connectionString)) {
                return rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                {
                    cartId = cartId,
                    productId = productId
                });
            }
        }
        // Remove -1 to quantity
        public async Task<int> RemoveQuantityOfProductInCart(int productId, int cartId)
        {
            int rowsAffected;
            string sqlCommand = "update CartProducts set quantity = quantity - 1 where cartId = @cartId and productId = @productId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                {
                    cartId = cartId,
                    productId = productId
                });
            }
        }
        // Get quantity of a product in cart
        public async Task<int> GetQuantityOfProductInCart(int productId, int cartId)
        {
            int quantityOfProduct;
            string sqlCommand = "select quantity from CartProducts where cartId = @cartId and productId = @productId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return quantityOfProduct = await connection.QueryFirstOrDefaultAsync<int>(sqlCommand, new
                {
                    cartId = cartId,
                    productId = productId
                });
            }
        }

        // Getting the number of products in a user's cart
        public async Task<IEnumerable<int>> getNumberOfProductsInCartFromDb(int userId) {                                   // Getting the number of items in cart
            IEnumerable<int> numberOfItems;
            string sqlCommand= "select count(productId) as NumberOfProducts from CartProducts inner join Cart on CartProducts.cartId = Cart.cartId where userId = @userId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return numberOfItems = await connection.QueryAsync<int>(sqlCommand, new {
                    userId = userId
                });
            }
        }

        // Delete all products from a user's cart
        public async Task<int> deleteProductsFromCartDbAsync(int userId) {
            var deletedProductsFromCart = 0;
            string sqlCommand = "delete CartProducts from CartProducts inner join Cart on CartProducts.cartId = Cart.cartId where userId = @userId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return deletedProductsFromCart =  await connection.ExecuteAsync(sqlCommand, new 
                {
                    userId = userId
                });
            }
        }

        // Delete a specific product from a user's cart
        public async Task<int> deleteSpecificProductFromCartDbAsync(int userId, int productId) {
            var deletedProductFromCart = 0;
            string sqlCommand = "delete top (1) CartProducts from CartProducts inner join Cart on CartProducts.cartId = Cart.cartId where userId = @userId and productId = @productId";
            using (var connection = new SqlConnection(_connectionString)) 
            {
                return deletedProductFromCart = await connection.ExecuteAsync(sqlCommand, new
                {
                    userId = userId,
                    productId = productId
                });
            }
        }
    }
}
