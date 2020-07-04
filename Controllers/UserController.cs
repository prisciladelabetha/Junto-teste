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
                return BadRequest(new { error = e.Message});
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<ActionResult> Signup([FromBody] SignupDto signupDto)
        {
            try
            {
                await this.UserService.Signup(signupDto.username, signupDto.password);
                return Ok( new { message = "Ok!"});
            }
            catch(Exception e)
            {
                return BadRequest(new { error = e.Message});
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                var username = User.Identity.Name;
                await this.UserService.ChangePassword(username, changePasswordDto.oldPassword, changePasswordDto.newPassword);
                return Ok( new { message = "Password changed"});
            }
            catch(Exception e)
            {
                return BadRequest(new { error = e.Message});
            }
        }

        public class ChangePasswordDto
        {
            public string oldPassword { get; set; }
            public string newPassword { get; set; }
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