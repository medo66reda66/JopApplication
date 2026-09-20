using Hmoe_Maintenance.Services;
using JopApplication.Application.Dtos;
using JopApplication.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication.Application.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> Register(RegisterRequest registerRequest);

        Task<LoginggResult> Login(LoginRequest loginRequest);
      

    }
}

