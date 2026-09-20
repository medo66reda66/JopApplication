using Ecommers.Api.Utilities;
using JopApplication.Domain.Models;
using JopApplication.infrastructure.Persistenc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JopApplication.infrastructure.Utilise
{
    public class DBIntializer : IDBIntializer
    {
        private readonly AppDBcontext _Context;
        private readonly ILogger<DBIntializer> _logger;
        private readonly UserManager<Appuser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;

        public DBIntializer(AppDBcontext context, RoleManager<IdentityRole> roleManager, UserManager<Appuser> userManager, ILogger<DBIntializer> logger)
        {
            _Context = context;
            _RoleManager = roleManager;
            _UserManager = userManager;
            _logger = logger;
        }

        public async Task Intialize()
        {
            try
            {
                if (_Context.Database.GetPendingMigrations().Any())
                {
                    _Context.Database.Migrate();
                }
                if (!_RoleManager.Roles.Any())
                {
                    _RoleManager.CreateAsync(new(DS.ADMIN_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.CANDIDATE_ROLE)).GetAwaiter().GetResult();
                    _RoleManager.CreateAsync(new(DS.RECRUITER_ROLE)).GetAwaiter().GetResult();

                    var result = await _UserManager.CreateAsync(new Appuser
                    {
                        UserName = "Admin",
                        Email = "Admin123@Hoda.com",
                        FirstName = "Mahmoud",
                        LastName = "Zahra",
                        PhoneNumber = "01120811023",
                        EmailConfirmed = true,
                    }, "admin123#");

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            Console.WriteLine(error.Description);
                    }

                    var user = await _UserManager.FindByEmailAsync("Admin123@Hoda.com");

                    await _UserManager.AddToRoleAsync(user!, DS.ADMIN_ROLE);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
        }
    }
}
