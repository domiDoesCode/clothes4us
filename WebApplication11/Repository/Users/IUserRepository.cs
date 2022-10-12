using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository
{
    public interface IUserRepository 
    {
        public Task<UserReturnable> verifyUserAtLoginAsync(User user);
        public Task<UserReturnable> getUserFromDbByIdAsync(int userId);
        public Task<UserReturnable> getUserFromDbByEmailAsync(string email);
        public Task<UserReturnable> getUserFromDbByUsernameAsync(string username);
        public Task<int> InsertUserToDbAsync(string username, string password, string email, string firstname, string lastname, DateTime createdAt);
        public Task<int> DeleteUserFromDb(int userId);
        public Task<int> UpdateUsersEmailInDb(string email, int userId);
        public Task<int> UpdateUsersPasswordInDb(string password, int userId);
        public  Task<IEnumerable<User>> getAllUsersFromDbAsync();
    }
}
