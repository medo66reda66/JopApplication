using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Application.Interfaces;
using JopApplication.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JopApplication.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Appuser> _userManager;
        private readonly ItokenService _itokenService;
        private readonly SignInManager<Appuser> _signInManager;

        public UserService(UserManager<Appuser> userManager, ItokenService itokenService, SignInManager<Appuser> signInResult)
        {
            _userManager = userManager;
            _itokenService = itokenService;
            _signInManager = signInResult;
        }

        public async Task<IdentityResult> Register(RegisterRequest registerRequest)
        {
            var user = new Appuser
            {
                FirstName = registerRequest.Firestname,
                LastName = registerRequest.Lastname,
                Address = registerRequest.Address,
                City = registerRequest.City,
                Country = registerRequest.Country,
                Email = registerRequest.Email,
                UserName = registerRequest.Username,
                Phone = registerRequest.Phone,
                EmailConfirmed = true
            };
         
            var result  =await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            if(registerRequest.role == Role.Candidate)
            {
               await _userManager.AddToRoleAsync(user, DS.CANDIDATE_ROLE);
            }
            if (registerRequest.role == Role.Recruiter)
            {
                await _userManager.AddToRoleAsync(user, DS.RECRUITER_ROLE);
            }

            return result;
        }
        public async Task<LoginggResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null) return new LoginggResult { Status = LoginStatus.UserNotFound };

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, loginRequest.RememberMe, true);

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

  