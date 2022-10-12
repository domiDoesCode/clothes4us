using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Managers;
using WebApplication11.Model;
using WebApplication11.Repository.Cart;
using WebApplication11.Repository.Orders;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication11.Controllers.Orders
{
    [Route("api/User/{userId}/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        public OrderController(IJWTAuthenticationManager iJWTAuthenticationManager, ICartRepository cartRepository, IOrderRepository orderRepository) { 
            _jwtAuthenticationManager = iJWTAuthenticationManager;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrderIntoDb(int userId) {
            int cartId = await _cartRepository.GetCartIdOfUserFromDb(userId);
            int rowsAffected = await _orderRepository.InsertOrderToDbAsync(userId, DateTime.Now, cartId);
            if (rowsAffected != 0) {
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IEnumerable<Order>> getAllOrdersFromDbAsync()
        {
            var orders = await _orderRepository.getAllOrdersFromDbAsync();
            return orders;
        }


        // Delete order from Db
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrderfromDb(int orderId)
        {
            try
            {
                await _orderRepository.DeleteOrderFromDb(orderId);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        // Update order's orderDate
        [Route("{orderId}/orderdate")]
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderDate(int orderId, [FromBody] Order order)
        {
            try
            {
                await _orderRepository.UpdateOrderDateInDb(order.orderDate, orderId);
                return Ok();
            }
            catch { return BadRequest(); }
        }

        [HttpGet("{orderId}")]
        public async Task<Order> GetOrder(int orderId)
        {
            try
            {
                return await _orderRepository.getOrderFromDbByIdAsync(orderId);
            }
            catch { return null; }
        }
    }
}
