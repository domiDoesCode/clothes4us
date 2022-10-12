using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Managers
{
    public interface IJWTAuthenticationManager
    {
        public Task<(string, UserReturnable user)> AuthenticateAsync(User user);
        public string ValidateToken(string token);
    }
}
