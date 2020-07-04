using System.Threading.Tasks;
using Junto.Users.Domain;

namespace Junto.Users.Infrastructure
{
    public class UserRepositoryImpl : IUserRepository
    {
        public Task Create(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> FindByuserName(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}