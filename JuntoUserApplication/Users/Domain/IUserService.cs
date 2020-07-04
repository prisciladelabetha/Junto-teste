using System.Threading.Tasks;

namespace Junto.Users.Domain
{
    public interface IUserService
    {
        Task<AuthenticatedUser> Login(string username, string password);
        Task Signup(string username, string password);
        Task ChangePassword(string username, string oldPassoword, string newPassword);        
    }

}