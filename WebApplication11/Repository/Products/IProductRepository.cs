using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Products
{
    public interface IProductRepository
    {
        public Task<int> insertProductToDbAsync(string sizeId, string typeId, string brandId, string colourId, int priceId, string name, int quantity);
        public Task<IEnumerable<Product>> getAllProductsFromDbAsync();
        public  Task<int> DeleteProductFromDb(int productId);
        public  Task<int> UpdateProductQuantityInDb(int quantity, int productId);
        public  Task<int> UpdateProductPriceInDb(int price, int productId);
        public  Task<ProductReturnable> getProductFromDbByIdAsync(int productId);
    }
}
