using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication11.Model
{
    public class CartModel
    {
        public int cartId { get; set; }
        public int userId { get; set; }
        public string status { get; set; }
        public CartModel()
        {

        }
    }
}
