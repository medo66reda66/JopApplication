
using Hmoe_Maintenance.Services;
using JopApplication.Application.Interfaces;
using JopApplication.Application.Services;
using JopApplication.Domain.Models;
using JopApplication.infrastructure.Persistenc;
using JopApplication.infrastructure.Repository;
using JopApplication.infrastructure.Utilise;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.Tasks;

namespace JopApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
             ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<AppDBcontext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            builder.Services.AddIdentity<Appuser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDBcontext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = "https://localhost:7066",
                    ValidAudience            = "https://localhost:7066",
                    IssuerSigningKey         = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("Nf7$Pq19!sD@84LmZ#xT2wQvR%k9Hp36"))
                };
            });

            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IRepository<Jop>, Repository<Jop>>();
            builder.Services.AddScoped<IRepository<Candidate>, Repository<Candidate>>();
            builder.Services.AddScoped<IRepository<CandidatJopApplication>, Repository<CandidatJopApplication>>();
            builder.Services.AddScoped<IJopService, JopService>();
            builder.Services.AddScoped<ICandidateService , CandidateService>();
            builder.Services.AddScoped<ICandidateJopAppService , CandidateJopAppService>();
            builder.Services.AddScoped<ItokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDBIntializer, DBIntializer>();

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBIntializer>();
            await service!.Intialize();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
