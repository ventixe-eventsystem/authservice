namespace Business.Models;
public class AuthResponse
{
  public bool IsSuccess { get; set; }
  public string Token { get; set; } = null!;
  public User? User { get; set; }
  public string? Message { get; set; }
}

public class User
{
  public string UserId { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public IEnumerable<string> Roles { get; set; } = [];
}