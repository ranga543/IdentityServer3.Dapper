using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityServer3.Dapper.Sample.Auth.Models;

namespace IdentityServer3.Dapper.Sample.Auth.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(Func<IDbConnection> openConnection) : base(openConnection) {}

        public async Task<User> GetAsync(string username, string password)
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<User>("select * from [Users] where [Username]=@username and [Password]=@password", 
                    new { username, password });

                return queryResult.SingleOrDefault();
            }
        }
        public async Task<User> GetAsync(string username)
        {
            using (var connection = OpenConnection())
            {
                var queryResult = await connection.QueryAsync<User>("select * from [Users] where [Username]=@username",
                    new { username });

                return queryResult.SingleOrDefault();
            }
        }

        public async Task<bool> SaveAsync(User user)
        {
            using (var connection = OpenConnection())
            {
                user.Password = HashHelper.Sha512(user.Password);
                var queryResult = await connection.ExecuteAsync("INSERT INTO Users (Username, Password) VALUES (@Username, @Password)",
                    user);

                return true;
            }
        }
    }

    public interface IUserRepository
    {
        Task<User> GetAsync(string username, string password);
        Task<User> GetAsync(string username);
        Task<bool> SaveAsync(User user);
    }
}