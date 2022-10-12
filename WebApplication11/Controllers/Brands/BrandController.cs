using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Model;
using WebApplication11.Repository.Brands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication11.Controllers.Brands
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandController(IBrandRepository brandRepository) {
            _brandRepository = brandRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Brand>> getAllBrandsFromDbAsync()
        {
            return await _brandRepository.getAllBrandsFromDbAsync();
        }
    }
}
