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

        public async Task ChangePassword(string username, string oldPassoword, string newPassword)
        {
            if (username == null || oldPassoword == null)
                throw new Exception("Invalid username or password");

            if (newPassword.Length < 6)
                throw new Exception("Password must be have at least 6 characters.");

            var user = await this.UserRepository.FindByuserName(username);
            if(user == null || !BCrypt.Net.BCrypt.Verify(oldPassoword, user.Password))
                throw new Exception("Invalid username or password");
            
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            
            await this.UserRepository.Update(user);

        }

        public async Task<AuthenticatedUser> Login(string username, string password)
        {
            if (username == null || password == null)
                throw new Exception("Invalid username or password");

            var user = await this.UserRepository.FindByuserName(username);
            if(BCrypt.Net.BCrypt.Verify(password, user.Password))
                return new AuthenticatedUser(user.Id, user.Username, GenerateToken(user.Username));

            throw new Exception("Invalid username or password");
        }

        public async Task Signup(string username, string password)
        {
            if (username == null || password == null)
                throw new Exception("Invalid username or password");

            if (password.Length < 6)
                throw new Exception("Password must be have at least 6 characters.");

            if (username.Length < 6)
                throw new Exception("Username must be have at least 6 characters.");    

            if(await this.UserRepository.FindByuserName(username) != null)
                throw new Exception("User already registered");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            await this.UserRepository.Create(new User(Guid.NewGuid(), username, hashedPassword));
        }

        private string GenerateToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(this.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, username)
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