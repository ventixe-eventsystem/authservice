using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;
public class VerificationService(UserManager<UserEntity> userManager) : IVerificationService
{
  private readonly UserManager<UserEntity> _userManager = userManager;

  public async Task<IdentityResult> VerifyEmailAsync(string token, string email)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
      return IdentityResult.Failed(new IdentityError { Description = "User not found" });

    var result = await _userManager.ConfirmEmailAsync(user, token);
    return result;
  }
}
