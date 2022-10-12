using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Managers;
using WebApplication11.Model;
using WebApplication11.Repository.Addresses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication11.Controllers.Orders
{
    [Route("api/User")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IJWTAuthenticationManager _JWTAuthenticationManager;
        public ShippingController(IAddressRepository addressRepository, IJWTAuthenticationManager jWtAuthenticationManager) { 
            _JWTAuthenticationManager = jWtAuthenticationManager;
            _addressRepository = addressRepository;
        }
        //[Route("{userId}/address")]
        [HttpPost("{userId}/address")]
        public async Task<IActionResult> InsertShippingAddressToDb(int userId, [FromBody] Address address) {
            int rowsAffected = await _addressRepository.InsertShippingAddressToDbAsync(address.country, address.city, address.address, userId);
            if (rowsAffected == 1) {
                return Ok();
            }
            return BadRequest();
        }
        //[Route("{userId}/address")]
        [HttpGet("{userId}/address")]
        public async Task<Address> GetUsersAddressFromDbByUserId(int userId) {
            var token = Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;                        // Validating the user first
            int userId2 = int.Parse(_JWTAuthenticationManager.ValidateToken(token));                         // and getting the userId from token

            var address = await _addressRepository.GetUsersAddressFromDbByUserIdAsync(userId);              // Getting the address by userId 
            return address;
        }

        [HttpPut("{userId}/address")]
        public async Task<IActionResult> UpdateUsersAddressInDb(int userId, [FromBody] Address address) {
            try
            {
                int rowsAffected = await _addressRepository.UpdateUsersAddressInDbAsync(address.country, address.city, address.address, userId);    // Getting the rowsAffected when updating the address
                if (rowsAffected == 1)
                {                                                                        // If thats 1 return Ok
                    return Ok();
                }
            }
            catch (Exception ex){ 
                string a = ex.StackTrace; 
                return BadRequest(); }
            return BadRequest();
        }
    }
}
