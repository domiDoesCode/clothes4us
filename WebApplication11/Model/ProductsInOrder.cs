namespace WebApplication11.Model
{
    public class ProductsInOrder
    {
        public int productsInOrderId { get; set; }
        public int productId { get; set; }
        public int userId { get; set; }
        public int orderId { get; set; }
        public ProductsInOrder() { 
        }
    }
}
