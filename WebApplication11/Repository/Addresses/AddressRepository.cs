using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Addresses
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AddressRepository(IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");                      // Getting the connection string from appsettings.json
        }

        // Inserting address into Db
        public async Task<int> InsertShippingAddressToDbAsync(string country, string city, string address, int userId)
        {
            int rowsAffected;
            string sqlCommand = "insert into Addresses (country, city, address, userId) values (@country, @city, @address, @userId)";
            using (var connection = new SqlConnection(_connectionString)) {
                return rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                {
                    country = country,
                    city = city,
                    address = address,
                    userId = userId
                });
            }
        }

        // Getting a user's address from Db by userId
        public async Task<Address> GetUsersAddressFromDbByUserIdAsync(int userId) {
            Address address;
            string sqlCommand = "select * from Addresses where userId=@userId";
            using (var connection = new SqlConnection(_connectionString)) {
                return address = await connection.QueryFirstOrDefaultAsync<Address>(sqlCommand, new
                {
                    userId = userId
                });
            }
        }

        // Update a user's address in Db by userId
        public async Task<int> UpdateUsersAddressInDbAsync(string country, string city, string address, int userId) {
            int rowsAffected;
            string sqlCommand = "update Addresses set country=@country, city=@city, address=@address where userId=@userId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    rowsAffected = await connection.ExecuteAsync(sqlCommand, new
                    {
                        userId = userId,
                        country = country,
                        city = city,
                        address = address
                    });
                    if (rowsAffected == 0) {
                        throw new Exception();
                    }
                    return rowsAffected;
                }
            }
            catch { throw new Exception(); }
            }
    }
}
