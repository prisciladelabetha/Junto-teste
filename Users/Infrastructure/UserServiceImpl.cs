using System;
using System.Threading.Tasks;
using Junto.Users.Domain;

namespace Junto.Users.Infrastructure
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository UserRepository;

        public UserServiceImpl(IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }
        public Task ChangePassword(string username, string oldPassoword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Login(string username, string password)
        {
            return await Task.Run( () => new User(Guid.NewGuid(), username, password));
        }

        public Task Signup(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}