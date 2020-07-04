using System.Threading.Tasks;

namespace Junto.Users.Domain
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task Update(User user);
        Task<User> FindByuserName(string username);
    }
}