using System.ComponentModel.DataAnnotations;

namespace DotnetFoundation.Domain.Entities;

public class User
{
  public int Id;
  public string FirstName { get; set; } = null!;
  public string? LastName { get; set; }
}