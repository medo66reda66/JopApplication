using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Features.Accounts.Commands.LoginAccount
{
    public class LoginAccountHandler : IRequestHandler<LoginAccountCommand, LoginggResult>
    {
        private readonly UserManager<Appuser> _userManager;
        private readonly ItokenService _itokenService;
        private readonly SignInManager<Appuser> _signInManager;

        public LoginAccountHandler(UserManager<Appuser> userManager, ItokenService itokenService, SignInManager<Appuser> signInResult)
        {
            _userManager = userManager;
            _itokenService = itokenService;
            _signInManager = signInResult;
        }

        public async Task<LoginggResult> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) return new LoginggResult { Status = LoginStatus.UserNotFound };

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (result.IsLockedOut)
            {
                return new LoginggResult { Status = LoginStatus.LockedOut };
            }
            else if (!user.EmailConfirmed)
            {
                return new LoginggResult { Status = LoginStatus.EmailNotConfirmed };
            }
            else if (!result.Succeeded)
            {
                return new LoginggResult { Status = LoginStatus.InvalidPassword };
            }


            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = _itokenService.CreateToken(claims);
            await _userManager.UpdateAsync(user);

            return new LoginggResult
            {
                Status = LoginStatus.Success,
                Token = token,
                Validtoken = "30 minutes"
            };
        }


    }
}
