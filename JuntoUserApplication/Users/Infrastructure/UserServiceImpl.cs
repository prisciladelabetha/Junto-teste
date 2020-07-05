using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Junto.Users.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Junto.Users.Infrastructure
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly IPasswordService PasswordService;
        private readonly JwtSettings JwtSettings;

        public UserServiceImpl(IUserRepository userRepository,
            IPasswordService passwordService,
            IOptions<JwtSettings> jwtSettingsAccessor)
        {
            this.UserRepository = userRepository;
            this.JwtSettings = jwtSettingsAccessor.Value;
            this.PasswordService = passwordService;
        }

        public async Task ChangePassword(string username, string oldPassoword, string newPassword)
        {
            if (username == null || oldPassoword == null)
                throw new Exception("Invalid password");

            if (newPassword.Length < 6)
                throw new Exception("Password must be have at least 6 characters.");

            var user = await this.UserRepository.FindByUsername(username);
            if(user == null || !this.PasswordService.VerifyPassword(oldPassoword, user.Password))
                throw new Exception("Invalid password.");
            
            user.Password = this.PasswordService.HashPassword(newPassword);
            
            await this.UserRepository.Update(user);

        }

        public async Task<AuthenticatedUser> Login(string username, string password)
        {
            if (username == null || password == null)
                throw new Exception("Invalid username or password");

            var user = await this.UserRepository.FindByUsername(username);
            if(this.PasswordService.VerifyPassword(password, user.Password))
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

            if(await this.UserRepository.FindByUsername(username) != null)
                throw new Exception("User already registered");

            var hashedPassword = this.PasswordService.HashPassword(password);

            await this.UserRepository.Create(new User(Guid.NewGuid(), username, hashedPassword));
        }

        private string GenerateToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(this.JwtSettings.JwtSecret);

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