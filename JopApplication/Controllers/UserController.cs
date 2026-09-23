using JopApplication.Application.Dtos;
using JopApplication.Application.Features.Accounts.Commands.LoginAccount;
using JopApplication.Application.Features.Accounts.Commands.RegisterAccount;
using JopApplication.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _mediator.Send(new RegisterAccountCommand()
            {
                Firestname = registerRequest.Firestname,
                Lastname = registerRequest.Lastname,
                Address = registerRequest.Address,
                City = registerRequest.City,
                ConfirmPassword = registerRequest.ConfirmPassword,
                Country = registerRequest.Country,
                Email = registerRequest.Email,
                Phone = registerRequest.Phone,
                Username = registerRequest.Username,
                Password = registerRequest.Password,
                role = registerRequest.role,
            });

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
            var result = await _mediator.Send(new LoginAccountCommand()
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password,
                RememberMe = loginRequest.RememberMe
            });

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
