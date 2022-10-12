using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Cart
{
    public interface ICartRepository
    {
        public Task<int> GetNumberOfCartOfUserFromDbAsync(int userId);
        public Task<int> InsertCartToDbByUserIdAsync(int userId);
        public Task<int> InsertProductToCartDb(int cartId, int productId);
        public Task<int> GetCartIdOfUserFromDb(int userId);
        public Task<IEnumerable<Product>> getAllProductsInCartFromDbAsync(int userId);
        public Task<int> deleteProductsFromCartDbAsync(int userId);
        public Task<IEnumerable<int>> getNumberOfProductsInCartFromDb(int userId);
        public Task<int> deleteSpecificProductFromCartDbAsync(int userId, int productId);
        public Task<int> AddQuantityOfProductInCart(int productId, int cartId);
        public Task<int> RemoveQuantityOfProductInCart(int productId, int cartId);
        public Task<int> GetQuantityOfProductInCart(int productId, int cartId);
    }
}
