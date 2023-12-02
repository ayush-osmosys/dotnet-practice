using DotnetFoundation.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetFoundation.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;
  public UserController(IUserService userService)
  {
    _userService = userService;
  }


  [HttpGet]
  [Authorize]
  public async Task<IActionResult> GetAllUsersAsync()
  {
    var result = await _userService.GetAllUsersAsync();
    return Ok(result);
  }

  [HttpGet("{userId}")]
  public async Task<IActionResult> GetUserByIdAsync(int userId)
  {
    var result = await _userService.GetUserByIdAsync(userId);
    return Ok(result);
  }
}