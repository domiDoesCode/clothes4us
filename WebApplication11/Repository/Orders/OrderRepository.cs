using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication11.Model;
using WebApplication11.Repository.Cart;

namespace WebApplication11.Repository.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        public OrderRepository(IConfiguration configuration, ICartRepository cartRepository) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");                      // Getting the connection string from appsettings.json
            _cartRepository = cartRepository;
        }

        //Insert order to Db
        public async Task<int> InsertOrderToDbAsync(int userId, DateTime orderDate, int cartId) {
            int rowsAffected;
            IEnumerable<Product> products = await _cartRepository.getAllProductsInCartFromDbAsync(cartId);
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.RepeatableRead))
                {
                    try
                    {
                        // Step1;
                        foreach (Product product in products)
                        {
                            string sqlCommandSelectQuantity = "select quantity from CartProducts where cartId = @cartId and productId = @productId";
                            int quantity =await connection.QueryFirstAsync<int>(sqlCommandSelectQuantity, new
                            {
                                cartId = cartId,
                                productId = product.productId
                            }, transaction: transaction);
                            if (product.stockQuantity - quantity < 0)
                            {
                                throw new Exception();
                            }
                            string sqlCommandUpdateQuantity = "update Products set quantity = quantity - @cartQuantity where productId = @productId";
                            await connection.ExecuteAsync(sqlCommandUpdateQuantity, new
                            {
                                cartQuantity = quantity,
                                productId = product.productId
                            }, transaction: transaction);
                        }

                        //Step2
                        string sqlCommandUpdateCartStatus = "update Cart set status = 'closed' where cartId = @cartId";
                        await connection.ExecuteAsync(sqlCommandUpdateCartStatus, new
                        {
                            cartId = cartId
                        }, transaction: transaction);
                        //Step3
                        string sqlCommandInsertOrder = "insert into Orders (userId, orderDate, cartId) values (@userId, @orderDate, @cartId)";
                        rowsAffected = await connection.ExecuteAsync(sqlCommandInsertOrder, new
                        {
                            userId = userId,
                            orderDate = orderDate,
                            cartId = cartId
                        }, transaction: transaction);
                        transaction.Commit();
                        return rowsAffected;
                    }
                    //Rollback on catch
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        // Get all orders 
        public async Task<IEnumerable<Order>> getAllOrdersFromDbAsync()
        {
            IEnumerable<Order> orders;
            string sqlCommand = "select * from Orders";
            using (var connection = new SqlConnection(_connectionString))
            {
                return orders = await connection.QueryAsync<Order>(sqlCommand);
            }
        }

        // Delete Order from DB 
        public async Task<int> DeleteOrderFromDb(int orderId)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("delete Orders from Orders where orderId = @orderId", new
                        {
                            orderId = orderId
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
        // Update Order's orderDate in DB
        public async Task<int> UpdateOrderDateInDb(DateTime orderDate, int orderId)
        {
            string sqlCommand = "update Orders set orderDate=@orderDate where orderId = @orderId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.ExecuteAsync(sqlCommand, new
                    {
                        orderDate = orderDate,
                        orderId = orderId
                    });
                }
            }
            catch { throw new Exception("Order not changed"); }
        }

        // Get Order by orderId
        public async Task<Order> getOrderFromDbByIdAsync(int orderId)
        {
            Order returnedOrderById;
            string sqlCommand = "Select orderId, userId, orderDate, cartId from [Orders] where orderId = @orderId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return returnedOrderById = await connection.QueryFirstOrDefaultAsync<Order>(sqlCommand, new
                {
                    orderId = orderId
                });
            }
        }
    }
}
