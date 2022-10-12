using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebApplication11.Managers;
using WebApplication11.Model;
using WebApplication11.Repository.Cart;

namespace WebApplication11.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJWTAuthenticationManager _JWTAuthenticationManager;
        private readonly ICartRepository _cartRepository;
        public LoginController(IJWTAuthenticationManager jWTAuthenticationManager, ICartRepository cartRepository) {
            _JWTAuthenticationManager = jWTAuthenticationManager;
            _cartRepository = cartRepository;
        }

        [HttpPost]                                                                      // Getting the token
        public async Task<IActionResult> LoginUserAsync([FromBody] User user)
        {
            var result = await _JWTAuthenticationManager.AuthenticateAsync(user);       // Authentication the user
            if (result.Item2 == null && result.Item1 == null) { 
                return Unauthorized();
            }

            int numberOfCartOfUser = await _cartRepository.GetNumberOfCartOfUserFromDbAsync(result.Item2.userId);
            if (numberOfCartOfUser == 0) {                                              // Checking if cart exists at login
                await _cartRepository.InsertCartToDbByUserIdAsync(result.Item2.userId); // If not, create one
            }

            Response.Cookies.Append(                                                // Putting the token into a cookie
                "token",
                result.Item1                                                        // Using Item1 of the return   
            ) ;
            return Ok(result.Item2);                                                // Using Item2 of the return
        }
    }
}
