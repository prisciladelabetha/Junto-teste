using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Junto.Users.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Junto.Users.Infrastructure
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly string JwtSecret;

        public UserServiceImpl(IUserRepository userRepository, IConfiguration configuration)
        {
            this.UserRepository = userRepository;
            this.JwtSecret = configuration.GetValue<string>("JwtSecret");
        }

        public Task ChangePassword(string username, string oldPassoword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuthenticatedUser> Login(string username, string password)
        {
            var user = await this.UserRepository.FindByuserName(username);
            if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                return new AuthenticatedUser(user.Id, user.Username, GenerateToken(user.Username));

            throw new Exception("Invalid username or password");
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

        private string GenerateToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(this.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("username", username)
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}