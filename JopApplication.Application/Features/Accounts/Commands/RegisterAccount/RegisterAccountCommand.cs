using JopApplication.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Features.Accounts.Commands.RegisterAccount
{
    public class RegisterAccountCommand : IRequest<IdentityResult>
    {
            [Required]
            public string Firestname { get; set; } = string.Empty;
            [Required]
            public string Lastname { get; set; } = string.Empty;
            [Required]
            public string Username { get; set; } = string.Empty;
            public string? Address { get; set; } = string.Empty;
            [Required]
            public string City { get; set; } = string.Empty;
            [Required]
            public string Country { get; set; } = string.Empty;
            [Required]
            public string Phone { get; set; } = string.Empty;

            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Confirm password is required")]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; } = string.Empty;

            public Role role { get; set; }
        
    }
}
