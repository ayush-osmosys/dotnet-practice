using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotnetFoundation.Application.DTO.AuthenticationDTO;
using DotnetFoundation.Application.Interfaces.Persistence;
using DotnetFoundation.Domain.Entities;
using DotnetFoundation.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DotnetFoundation.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
  private readonly IConfiguration _configuration;
  private readonly SqlDatabaseContext _dbContext;
  private readonly SignInManager<IdentityApplicationUser> _signInManager;
  private readonly UserManager<IdentityApplicationUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  public UserRepository(IConfiguration configuration, SqlDatabaseContext sqlDatabaseContext, SignInManager<IdentityApplicationUser> signinManager, RoleManager<IdentityRole> roleManager, UserManager<IdentityApplicationUser> userManager)
  {
    _dbContext = sqlDatabaseContext;
    _configuration = configuration;
    _roleManager = roleManager;
    _signInManager = signinManager;
    _userManager = userManager;

  }
  public string GenerateJwtToken(UserInfo user)
  {
    var claims = new List<Claim>
        {
            new(ClaimTypes.Email,user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            // Add additional claims for roles
        };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        _configuration["Jwt:Issuer"],
        _configuration["Jwt:Audience"],
        claims,
        expires: DateTime.Now.AddHours(2),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
  public async Task<string> AddUserAsync(RegisterRequest request)
  {
    IdentityApplicationUser newIdentityApplicationUser = new()
    {
      UserName = request.Email,
      Email = request.Email
    };
    var identityResult = await _userManager.CreateAsync(newIdentityApplicationUser, request.Password);
    var identityApplicationUser = await _userManager.FindByEmailAsync(request.Email) ?? throw new Exception("Error creating user");
    ApplicationUser applicationUser = new()
    {
      FirstName = request.FirstName,
      LastName = request.LastName,
      IdentityApplicationUserId = identityApplicationUser.Id
    };
    var res = await _dbContext.ApplicationUsers.AddAsync(applicationUser);
    await _dbContext.SaveChangesAsync();
    UserInfo userInfo = new(identityApplicationUser.Id, request.Email);
    return GenerateJwtToken(userInfo);

  }

  public async Task<List<User>> GetAllUsersAsync()
  {
    var users = (await _dbContext.ApplicationUsers.ToListAsync()).Select(user => new User { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName }).ToList();
    return users;


  }

  public async Task<User?> GetUserByIdAsync(int Id)
  {
    var user = await _dbContext.ApplicationUsers.FindAsync(Id);
    return user;
  }

  public async Task<string> LoginUserAsync(LoginRequest request)
  {
    var signInResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
    if (!signInResult.Succeeded)
    {
      throw new Exception("Invalid Email or Password");
    }
    IdentityApplicationUser user = await _userManager.FindByEmailAsync(request.Email) ?? throw new Exception("User does not exist");
    UserInfo userInfo = new(user.Id, request.Email);
    return GenerateJwtToken(userInfo);
  }

  public async Task<string> ForgotPasswordAsync(string email)
  {
    var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("Invalid Email");
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    return token;
  }

  public async Task<string> ResetPasswordAsync(string email, string token, string newPassword)
  {
    var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("Invalid Email");
    var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
    if (!result.Succeeded) throw new Exception("Invalid token");
    UserInfo userInfo = new(user.Id, email);
    return GenerateJwtToken(userInfo);
  }
}