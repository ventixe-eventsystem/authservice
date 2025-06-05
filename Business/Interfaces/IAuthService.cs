using Business.Models;

namespace Business.Interfaces;
public interface IAuthService
{
  Task<bool> EmailExistsAsync(string email);
  Task<AuthResponse> RegisterAsync(SignUpForm form);
  Task<AuthResponse> SignInAsync(SignInForm form);
}