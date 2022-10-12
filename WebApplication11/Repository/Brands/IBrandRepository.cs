using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository.Brands
{
    public interface IBrandRepository
    {
        public Task<IEnumerable<Brand>> getAllBrandsFromDbAsync();
    }
}
