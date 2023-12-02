using DotnetFoundation.Application.DTO.AuthenticationDTO;
using DotnetFoundation.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFoundation.Api.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthenticationController : ControllerBase
{
  private readonly IAuthenticationService _authenticationService;
  public AuthenticationController(IAuthenticationService authenticationService)
  {
    _authenticationService = authenticationService;
  }

  [HttpPost("regiser")]
  public async Task<IActionResult> RegisterAsync(RegisterRequest request)
  {
    var result = await _authenticationService.RegisterAsync(request);
    return Ok(result);
  }

  [HttpPost("login")]
  public async Task<IActionResult> LoginAsync(LoginRequest request)
  {
    var result = await _authenticationService.LoginAsync(request);
    return Ok(result);
  }
  [HttpPost("reset-password")]
  public async Task<IActionResult> ResetPasswordAsync(PasswordResetRequest request)
  {
    var result = await _authenticationService.ResetPasswordAsync(request);
    return Ok(result);
  }
  [HttpPost("forgot-password")]
  public async Task<IActionResult> ForgotPasswordAsync(string email)
  {
    var result = await _authenticationService.ForgotPasswordAsync(email);
    return Ok(result);
  }
}