using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication11.Model;
using WebApplication11.Repository;

namespace WebApplication11.Managers
{
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string key;
        private IConfiguration _configuration;
        private UserRepository _userRepository;
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();


        public JWTAuthenticationManager(string key, IConfiguration configuration)
        {
            this.key = key;
            _configuration = configuration;
            _userRepository = new UserRepository(_configuration);
        }
        public async Task<(string, UserReturnable user)> AuthenticateAsync(User user)
        {
            UserReturnable verifiedUser;
            try
            {
                verifiedUser = await _userRepository.verifyUserAtLoginAsync(user); 
            }
            catch (Exception ex) 
            {
                return (null, null);
            }

            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", verifiedUser.userId.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return (tokenHandler.WriteToken(token), verifiedUser);                                                                              // Returning multiple elements
        }

        public string ValidateToken(string token)
        {
            var tokenKey = Encoding.ASCII.GetBytes(key);
            if (token == null)
                throw new Exception("no user logged in");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == "userId").Value;
            }
            catch(Exception ex)
            {
                return null; 
            }
        }
    }
}
