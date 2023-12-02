using DotnetFoundation.Application.DTO.AuthenticationDTO;
using DotnetFoundation.Domain.Entities;

namespace DotnetFoundation.Application.Interfaces.Persistence;

public interface IUserRepository
{
  public Task<string> AddUserAsync(RegisterRequest request);
  public Task<string> LoginUserAsync(LoginRequest request);
  public Task<List<User>> GetAllUsersAsync();
  public Task<User?> GetUserByIdAsync(int Id);
  public Task<string> ForgotPasswordAsync(string email);
  public Task<string> ResetPasswordAsync(string email, string token, string newPassword);

}