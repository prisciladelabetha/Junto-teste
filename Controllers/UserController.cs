using System;
using System.Threading.Tasks;
using Junto.Users.Domain;
using Junto.Users.Infrastructure;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticatedUser>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await this.UserService.Login(loginDto.username, loginDto.password);
                return Ok(user);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
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