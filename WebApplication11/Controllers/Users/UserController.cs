using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication11.Managers;
using WebApplication11.Model;
using WebApplication11.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication11.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJWTAuthenticationManager _JWTAuthenticationManager;
        private readonly IUserRepository _userRepository;
        public UserController(IJWTAuthenticationManager jWTAuthenticationManager, IUserRepository userRepository) {
            _JWTAuthenticationManager = jWTAuthenticationManager;
            _userRepository = userRepository;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var token = Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;
            int userId = int.Parse(_JWTAuthenticationManager.ValidateToken(token));
            UserReturnable userReturnable = await _userRepository.getUserFromDbByIdAsync(userId);
            return Ok(userReturnable);
        }

        [HttpPost("logout")]
        public void DeleteMe() {
            var currentCookie = HttpContext.Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;
            Response.Cookies.Delete("token");
        }

        // Delete user from Db
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserfromDb(int userId) {
            try
            {
                await _userRepository.DeleteUserFromDb(userId);
                return Ok();
            }
            catch {
                return BadRequest();
            }
        }
        // Update user's email
        [Route("{userId}/email")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateEmailOfUser(int userId, [FromBody] User user) {
            
            //var token = Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;
            try
            {
                //int loggedInUserId = int.Parse(_JWTAuthenticationManager.ValidateToken(token));
                await _userRepository.UpdateUsersEmailInDb(user.email, userId);
                return Ok();
            }
            catch { return BadRequest(); }
        }

        [Route("{userId}/password")]
        [HttpPut]
        public async Task<IActionResult> UpdatePasswordOfUser(int userId, [FromBody] User user)
        {
            //var token = Request.Cookies.FirstOrDefault(x => x.Key == "token").Value;
            try
            {
                //int loggedInUserId = int.Parse(_JWTAuthenticationManager.ValidateToken(token));
                await _userRepository.UpdateUsersPasswordInDb(user.password, userId);
                return Ok();
            }
            catch { return BadRequest(); }
        }
        [HttpGet("{userId}")]
        public async Task<UserReturnable> GetUser(int userId) { 
            try { 
                return await _userRepository.getUserFromDbByIdAsync(userId);
            }catch { return null; }
        }
        [HttpGet]
        public async Task<IEnumerable<User>> getAllUsersFromDbAsync()
        {
            var users = await _userRepository.getAllUsersFromDbAsync();
            return users;
        }
    }
}
