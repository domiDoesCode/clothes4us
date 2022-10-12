using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication11.Model;
using WebApplication11.Repository;
using WebApplication11.Repository.Cart;

namespace WebApplication11.Controllers.Users
{
    [Route("api/User")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        public RegistrationController(IUserRepository userRepository, ICartRepository cartRepository) {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAccountAsync([FromBody] User user)                                                                                 
        {
            // Getting user by email to check if email is taken or not
            try
            {
                UserReturnable returnedUserByEmail = await _userRepository.getUserFromDbByEmailAsync(user.email);              }
            catch (Exception ex) { return StatusCode(409, "Email taken"); }
            // Getting user by username to check if username is taken or not
            try
            {
                UserReturnable returnedUserByUsername = await _userRepository.getUserFromDbByUsernameAsync(user.username);   
            }
            catch (Exception ex) { return StatusCode(400, "Username taken"); }

            DateTime createdAt = DateTime.Now;
            try
            {
                await _userRepository.InsertUserToDbAsync(user.username, user.password, user.email, user.firstname, user.lastname, createdAt);
                    return Created("Successful", user);               // Return created if no errors
            }
            catch (Exception ex) {
                return BadRequest();                                     // return BadRequest if catching error
            }
        }
    }
}
