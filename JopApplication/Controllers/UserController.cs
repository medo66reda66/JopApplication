using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _userService.Register(registerRequest);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "User registration failed.",
                    errors = result.Errors
                });
            }
            else
            {
                return Ok(new { message = "User registered successfully." });
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _userService.Login(loginRequest);

            switch (result.Status)
            {
                case LoginStatus.UserNotFound:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "User not found." }
                    });
                case LoginStatus.InvalidPassword:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "Invalid password." }
                    });
                case LoginStatus.EmailNotConfirmed:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "Email not confirmed." }
                    });
                case LoginStatus.LockedOut:
                    return BadRequest(new
                    {
                        message = "Login failed.",
                        errors = new[] { "User account is locked out." }
                    });
            }

            return Ok(new
            {
                message = "Login successful.",
                token = result.Token,
                validToken = result.Validtoken
            });
        }
    }
}
