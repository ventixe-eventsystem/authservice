using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services;
public class AuthService(UserManager<UserEntity> userManager, IConfiguration config, IEmailService emailService)
{
  private readonly IConfiguration _config = config;
  private readonly UserManager<UserEntity> _userManager = userManager;
  private readonly IEmailService _emailService = emailService;

  public async Task<AuthResponse> SignInAsync(SignInForm form)
  {
    var user = await _userManager.FindByEmailAsync(form.Email);
    if (user == null || !await _userManager.CheckPasswordAsync(user, form.Password))
    {
      return new AuthResponse
      {
        IsSuccess = false,
        Message = "Invalid credentials"
      };
    }

    var token = await GenerateJwtToken(user);
    var roles = await _userManager.GetRolesAsync(user);

    return new AuthResponse
    {
      IsSuccess = true,
      User = new User
      {
        Email = user.Email!,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Roles = [.. roles],
      },
      Token = token,
    };
  }

  public async Task<AuthResponse> RegisterAsync(SignUpForm form)
  {
    var user = new UserEntity
    {
      UserName = form.Email,
      FirstName = form.FirstName,
      LastName = form.LastName,
      Email = form.Email,
    };
    var result = await _userManager.CreateAsync(user, form.Password);
    if (!result.Succeeded)
    {
      return new AuthResponse
      {
        IsSuccess = false,
        Message = string.Join(", ", result.Errors.Select(e => e.Description))
      };
    }
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    await _userManager.AddToRoleAsync(user, "User");
    await _emailService.SendVerificationEmailAsync(user.Email!, token);
    return new AuthResponse
    {
      IsSuccess = true,
      User = new User
      {
        Email = user.Email!,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Roles = [],
      },
      Token = token,
    };
  }

  public async Task<string> GenerateJwtToken(UserEntity user)
  {
    var claims = new List<Claim>
    {
      new(ClaimTypes.NameIdentifier, user.Id),
      new(ClaimTypes.Email, user.Email!),
      new(ClaimTypes.Name, user.FirstName),
    };

    var roles = await _userManager.GetRolesAsync(user);
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _config["Jwt:Issuer"],
      audience: _config["Jwt:Audience"],
      claims: claims,
      expires: DateTime.Now.AddMinutes(30),
      signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}
