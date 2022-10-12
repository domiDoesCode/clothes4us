using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Products
{
    public class ProductRepository : IProductRepository
    {
        private string _connectionString;
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");                      // Getting the connection string from appsettings.json
        }

        // Insert User into DB
        public async Task<int> insertProductToDbAsync(string size, string type, string brand, string colour, int price, string name, int quantity)
        {
            int rowsAffected;
            string sqlCommand = "insert into Products (size, type, brand, colour, price, name, quantity) values(@size, @type, @brand, @colour, @price, @name, @quantity)";
            using (var connection = new SqlConnection(_connectionString))
            {
                return rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                {
                    size = size,
                    type = type,
                    brand = brand,
                    colour = colour,
                    price = price,
                    name = name,
                    quantity = quantity
                });
            }
        }

        // Getting all products from DB
        public async Task<IEnumerable<Product>> getAllProductsFromDbAsync()
        {
            IEnumerable<Product> products;
            string sqlCommand = "select * from Products";
            using (var connection = new SqlConnection(_connectionString)) {
                return products = await connection.QueryAsync<Product>(sqlCommand);
            }
        }
        public async Task<int> DeleteProductFromDb(int productId)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("delete Products from Products where productId = @productId", new
                        {
                            productId = productId
                        }, transaction: transaction);
                        rowsAffected++;

                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        // Update Products's quantity in DB
        public async Task<int> UpdateProductQuantityInDb(int quantity, int productId)
        {
            string sqlCommand = "update Products set quantity=@quantity where productId = @productId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.ExecuteAsync(sqlCommand, new
                    {
                        quantity = quantity,
                        productId = productId
                    });
                }
            }
            catch { throw new Exception("Quantity not changed"); }
        }
        // Update Products's price in DB
        public async Task<int> UpdateProductPriceInDb(int price, int productId)
        {

            string sqlCommand = "update Products set price = @price where productId = @productId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.ExecuteAsync(sqlCommand, new
                    {
                        price = price,
                        productId = productId
                    });
                }
            }
            catch { }
            return 0;
        }
        // Getting product from Db by Id
        public async Task<ProductReturnable> getProductFromDbByIdAsync(int productId)
        {
            ProductReturnable returnedProductById;
            string sqlCommand = "Select productId, size, type, brand, colour, price, name, quantity from [Products] where productId = @productId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return returnedProductById = await connection.QueryFirstOrDefaultAsync<ProductReturnable>(sqlCommand, new
                {
                    productId = productId
                });
            }
        }
    }
}
