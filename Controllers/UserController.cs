using System.Threading.Tasks;
using Junto.Users.Domain;
using Junto.Users.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Junto.Controllers
{    
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;

        public UserController(IUserService userService)
        {
            this.UserService = userService;
        }

        [HttpPost("login")]
        public Task<User> Login([FromBody] LoginDto loginDto)
        {
            return this.UserService.Login(loginDto.username, loginDto.password);
        }

        public class LoginDto
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}