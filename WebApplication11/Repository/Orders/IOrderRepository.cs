using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Orders
{
    public interface IOrderRepository
    {
        public Task<int> InsertOrderToDbAsync(int userId, DateTime orderDate, int cartId);
        public Task<IEnumerable<Order>> getAllOrdersFromDbAsync();
        public Task<int> DeleteOrderFromDb(int orderId);
        public Task<int> UpdateOrderDateInDb(DateTime orderDate, int orderId);
        public Task<Order> getOrderFromDbByIdAsync(int orderId);
    }
}
