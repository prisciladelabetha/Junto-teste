using System;
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

        [HttpPost("signup")]
        public async Task<ActionResult> Signup([FromBody] SignupDto signupDto)
        {
            try
            {
                await this.UserService.Signup(signupDto.username, signupDto.password);
                return Ok("OK!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        public class SignupDto
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class LoginDto
        {
            public string username { get; set; }
            public string password { get; set; }
        }
    }
}