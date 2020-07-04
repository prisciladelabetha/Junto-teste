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

        public async Task Signup(string username, string password)
        {
            if(await this.UserRepository.FindByuserName(username) != null)
            {
                throw new Exception("User already registered");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            await this.UserRepository.Create(new User(Guid.NewGuid(), username, hashedPassword));
        }
    }
}