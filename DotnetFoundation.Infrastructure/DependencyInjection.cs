using System.Text;
using DotnetFoundation.Application.Interfaces.Persistence;
using DotnetFoundation.Infrastructure.Identity;
using DotnetFoundation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DotnetFoundation.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
  {
    services.AddDbContext<SqlDatabaseContext>(options =>
    {
      string connectionString = configuration.GetConnectionString("DBConnection") ?? throw new Exception("Invalid connection string");
      options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });
    services.AddIdentity<IdentityApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SqlDatabaseContext>()
    .AddDefaultTokenProviders();

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
      };

    });
    services.AddAuthorization(options =>
    {
      options.AddPolicy("RequiredAdminRole", policy => policy.RequireRole("ADMIN"));
    });

    services.AddScoped<IUserRepository, UserRepository>();

    return services;
  }
}