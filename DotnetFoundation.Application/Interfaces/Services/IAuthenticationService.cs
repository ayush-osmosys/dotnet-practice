using DotnetFoundation.Application.DTO.AuthenticationDTO;
using DotnetFoundation.Domain.Entities;

namespace DotnetFoundation.Application.Interfaces.Services;

public interface IAuthenticationService
{
  public Task<AuthenticationResponse> RegisterAsync(RegisterRequest request);
  public Task<AuthenticationResponse> LoginAsync(LoginRequest request);
  public Task<string> ForgotPasswordAsync(string email);
  public Task<AuthenticationResponse> ResetPasswordAsync(PasswordResetRequest request);
}