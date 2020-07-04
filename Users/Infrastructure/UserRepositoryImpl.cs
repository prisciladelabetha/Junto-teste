using System.Data;
using System.Threading.Tasks;
using Dapper;
using Junto.Users.Domain;

namespace Junto.Users.Infrastructure
{
    public class UserRepositoryImpl : IUserRepository
    {

        private readonly IDbConnection DbConnection; 

        public UserRepositoryImpl(IDbConnection dbConnection)
        {
            this.DbConnection = dbConnection;
        }

        public async Task Create(User user)
        {
            await DbConnection.ExecuteAsync(@"
                INSERT INTO users
                    (id,
                    username,
                    password)
                VALUES
                    (@id,
                    @username,
                    @password)",
                new {
                    id = user.Id,
                    username = user.Username,
                    password = user.Password
            });
        }

        public Task<User> FindByuserName(string username)
        {
            return DbConnection.QuerySingleOrDefaultAsync<User>(@"
                SELECT * FROM 
                    users 
                WHERE 
                    username = @username",
                new { username });
        }

        public Task Update(User user)
        {
            return DbConnection.ExecuteAsync(@"
                UPDATE users
                SET 
                    password = @password
                WHERE
                    id = @id
                ",
                new { id = user.Id, password = user.Password });
        }
    }
}