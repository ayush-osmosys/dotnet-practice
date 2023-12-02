namespace DotnetFoundation.Infrastructure.Identity;
using DotnetFoundation.Domain.Entities;

public class ApplicationUser : User
{
  public string? IdentityApplicationUserId { get; set; }
  public IdentityApplicationUser? IdentityApplicationUser { get; set; }

}