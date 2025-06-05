using Microsoft.AspNetCore.Identity;

namespace Business.Interfaces;
public interface IVerificationService
{
  Task<IdentityResult> VerifyEmailAsync(string token, string email);
}