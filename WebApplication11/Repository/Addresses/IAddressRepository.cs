using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Addresses
{
    public interface IAddressRepository
    {
        public Task<int> InsertShippingAddressToDbAsync(string country, string city, string address, int userId);
        public Task<Address> GetUsersAddressFromDbByUserIdAsync(int userId);
        public Task<int> UpdateUsersAddressInDbAsync(string country, string city, string address, int userId);
    }
}
