using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Brands
{
    public class BrandRepository : IBrandRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public BrandRepository(IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");                      // Getting the connection string from appsettings.json
        }

        public async Task<IEnumerable<Brand>> getAllBrandsFromDbAsync()
        {
            IEnumerable<Brand> brands;
            string sqlCommand = "Select * from Brands";
            using (var connection = new SqlConnection(_connectionString)) {
                return  brands = await connection.QueryAsync<Brand>(sqlCommand);
            }
        }
    }
}
