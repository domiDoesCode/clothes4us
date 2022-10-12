using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApplication11.Model;

namespace WebApplication11.Repository
{
    public class UserRepository : IUserRepository
    {
        private string _connectionString;
        private readonly IConfiguration _configuration;
        public UserRepository(IConfiguration configuration) {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");                      // Getting the connection string from appsettings.json
        }

        private UserReturnable convertUserToReturnableUser(User user) {                                   // Convert user to return without password, and salt
            return new UserReturnable
            {
                userId = user.userId,
                username = user.username,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname,
                createdAt = user.createdAt
            };
        }

        // Getting user from DB by username and verify the password
        public async Task<UserReturnable> verifyUserAtLoginAsync(User user)
        {
            User returnedUser;
            string sqlCommand = "Select * from [Users] where username = @username";
            try {
                using (var connection = new SqlConnection(_connectionString)) {
                    returnedUser = await connection.QueryFirstAsync<User>(sqlCommand, new
                    {
                        username = user.username
                    });
                }
            } catch {
                throw new Exception("noUserFound");
            }

            string returnedSaltedPassword = user.password + returnedUser.passwordSalt;                               // Password salt is random, so we are getting the stored salt of the registered user
            bool passwordIsValid = BCrypt.Net.BCrypt.Verify(returnedSaltedPassword, returnedUser.password);
            if (passwordIsValid)
            {
                return convertUserToReturnableUser(returnedUser);                                                    // Converting user to UserReturnable
            }
            else {
                throw new Exception("invalid password");
            }
            
        }

        // Getting user from Db by Id
        public async Task<UserReturnable> getUserFromDbByIdAsync(int userId) {
            UserReturnable returnedUserById;
            string sqlCommand = "Select userId, username, email, firstname, lastname from [Users] where userId = @userId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return returnedUserById = await connection.QueryFirstOrDefaultAsync<UserReturnable>(sqlCommand, new
                {
                    userId = userId
                });
            }
        }

        // Getting user from Db by email
        public async Task<UserReturnable> getUserFromDbByEmailAsync(string email) {
            UserReturnable returnedUserByEmail;
            string sqlCommand = "Select userId, username, email, firstname, lastname from [Users] where email = @email";
                using (var connection = new SqlConnection(_connectionString))
                {
                    returnedUserByEmail = await connection.QueryFirstOrDefaultAsync<UserReturnable>(sqlCommand, new
                    {
                        email = email
                    });
                }
            if (returnedUserByEmail != null) { throw new Exception(); }
            return returnedUserByEmail;
        }

        // Getting user from Db by username
        public async Task<UserReturnable> getUserFromDbByUsernameAsync(string username) {
            UserReturnable returnedUserByUsername;
            string sqlCommand = "Select userId, username, email, firstname, lastnames from Users where username = @username";
            using (var connection = new SqlConnection(_connectionString)) {
                returnedUserByUsername = await connection.QueryFirstOrDefaultAsync<UserReturnable>(sqlCommand, new
                {
                    username = username
                });
                }
            if (returnedUserByUsername != null) { throw new Exception(); }
            return null;
        }


        // Insert User into DB
        public async Task<int> InsertUserToDbAsync(string username, string password, string email, string firstname, string lastname, DateTime createdAt)        // Execute returns the number of rows affected. Its 1 if user is created
        {
            Random rnd = new Random();                                                                                      // Generating random password salt                  
            var passwordSalt = "";
            for (var i = 0; i < 10; i++)
            {
                passwordSalt += ((char)(rnd.Next(1, 26) + 64)).ToString();
            }
            string saltedPassword = password + passwordSalt;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword); // Hashing the salted password
            try
            {
                string sqlCommand = "Insert into Users (username, password, email, firstname, lastname, passwordSalt, roleId, createdAt) values(@username, @password, @email, @firstname, @lastname, @passwordSalt, 1, @createdAt)";
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.ExecuteAsync(sqlCommand, new
                    {
                        username = username,
                        password = hashedPassword,
                        email = email,
                        firstname = firstname,
                        lastname = lastname,
                        passwordSalt = passwordSalt,
                        createdAt = createdAt
                    });
                }
            }
            catch { throw new Exception(); }
            }
        // Delete User from DB 
        public async Task<int> DeleteUserFromDb(int userId)
        {
            int rowsAffected = 0;
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                using (var transaction = connection.BeginTransaction()) {
                    try
                    {
                        await connection.ExecuteAsync("delete CartProducts from CartProducts where cartId = (select cartId from Cart where userId = @userId)", new 
                        {
                            userId = userId
                        }, transaction : transaction);
                        rowsAffected++;

                        await connection.ExecuteAsync("delete Cart from Cart where userId = @userId", new
                        {
                            userId = userId
                        }, transaction: transaction);
                        rowsAffected++;

                        await connection.ExecuteAsync("delete Orders from Orders where userId = @userId", new
                        {
                            userId = userId
                        }, transaction: transaction);
                        rowsAffected++;

                        await connection.ExecuteAsync("delete Addresses from Addresses where userId = @userId", new
                        {
                            userId = userId
                        }, transaction: transaction);
                        rowsAffected++;

                        await connection.ExecuteAsync("delete Users from Users where userId = @userId", new
                        {
                            userId = userId
                        }, transaction : transaction);
                        rowsAffected++;
                        transaction.Commit();
                        return rowsAffected;
                    }
                    catch (Exception ex){
                        transaction.Rollback();
                        string a = ex.Message;
                        throw new Exception("couldnt delete user");
                    }
                }
            }
        }
        // Update User's email in DB
        public async Task<int> UpdateUsersEmailInDb(string email, int userId) {
            string sqlCommand = "update Users set email=@email where userId = @userId";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.ExecuteAsync(sqlCommand, new
                    {
                        email = email,
                        userId = userId
                    });
                }
            }
            catch {throw new Exception("email not changed"); }
        }
        // Update User's password in DB
        public async Task<int> UpdateUsersPasswordInDb(string password, int userId) {
            Random rnd = new Random();                                                   // Generating random password salt                  
            var passwordSalt = "";
            for (var i = 0; i < 10; i++)
            {
                passwordSalt += ((char)(rnd.Next(1, 26) + 64)).ToString();
            }
            string saltedPassword = password + passwordSalt;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(saltedPassword); // Hashing the salted password

            string sqlCommand = "update Users set password = @hashedPassword, passwordSalt = @passwordSalt where userId = @userId";
            try {
                using (var connection = new SqlConnection(_connectionString)) {
                    return await connection.ExecuteAsync(sqlCommand, new
                    {
                        passwordSalt = passwordSalt,
                        hashedPassword = hashedPassword,
                        userId = userId
                    });
                }
            } catch { }
                return 0;
        }
        public async Task<IEnumerable<User>> getAllUsersFromDbAsync()
        {
            IEnumerable<User> users;
            string sqlCommand = "select * from Users";
            using (var connection = new SqlConnection(_connectionString))
            {
                return users = await connection.QueryAsync<User>(sqlCommand);
            }
        }
    }
}
