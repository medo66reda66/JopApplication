using Ecommers.Api.Utilities;
using JopApplication.Application.Dtos;
using JopApplication.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Features.Accounts.Commands.RegisterAccount
{
    public class RegisterAccountHandler : IRequestHandler<RegisterAccountCommand, IdentityResult>
    {
        private readonly UserManager<Appuser> _userManager;

        public RegisterAccountHandler(UserManager<Appuser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            var user = new Appuser
            {
                FirstName = request.Firestname,
                LastName = request.Lastname,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                Email = request.Email,
                UserName = request.Username,
                Phone = request.Phone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            if (request.role == Role.Candidate)
            {
                await _userManager.AddToRoleAsync(user, DS.CANDIDATE_ROLE);
            }
            if (request.role == Role.Recruiter)
            {
                await _userManager.AddToRoleAsync(user, DS.RECRUITER_ROLE);
            }

            return result;
        }
    }
}
