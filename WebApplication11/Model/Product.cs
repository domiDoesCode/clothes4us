using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication11.Model
{
    public class Product
    {
        public int productId { get; set; }
        public string size { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string colour { get; set; }
        public int price { get; set; }
        public string name { get; set; }
        public int stockQuantity { get; set; }
        public int quantity { get; set; }
        public Product() {                              // Constructor
        }
    }
}
