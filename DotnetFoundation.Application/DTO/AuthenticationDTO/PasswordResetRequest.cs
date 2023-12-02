namespace DotnetFoundation.Application.DTO.AuthenticationDTO;
public record PasswordResetRequest(string Email, string Token, string Password);