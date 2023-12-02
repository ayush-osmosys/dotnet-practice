using DotnetFoundation.Application.DTO.UserDTO;
namespace DotnetFoundation.Application.Interfaces.Services;

public interface IUserService
{
  public Task<UserResponse?> GetUserByIdAsync(int Id);
  public Task<List<UserResponse>> GetAllUsersAsync();
}