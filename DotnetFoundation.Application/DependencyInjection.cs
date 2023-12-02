namespace DotnetFoundation.Application;

using DotnetFoundation.Application.Interfaces.Services;
using DotnetFoundation.Application.Services.Authentication;
using DotnetFoundation.Application.Services.UserService;
using Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IUserService, UserService>();
    return services;
  }
}