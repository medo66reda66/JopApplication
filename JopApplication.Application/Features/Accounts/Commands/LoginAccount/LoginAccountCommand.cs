using JopApplication.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Features.Accounts.Commands.LoginAccount
{

    public class LoginAccountCommand : IRequest<LoginggResult>
    {

            [Required(ErrorMessage = "Email is required.")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required.")]

            public string Password { get; set; }

            public bool RememberMe { get; set; } = false;
            public LoginStatus Status { get; set; }
            public string Token { get; set; }
            public string Validtoken { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
            public string RefreshTokenTime { get; set; }
        
    }
}
