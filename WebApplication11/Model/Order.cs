using System;

namespace WebApplication11.Model
{
    public class Order
    {
        public int orderId { get; set; }
        public int userId { get; set; }
        public int totalPrice { get; set; }
        public int cartId { get; set; }
        public DateTime orderDate { get; set; }
    }
}
