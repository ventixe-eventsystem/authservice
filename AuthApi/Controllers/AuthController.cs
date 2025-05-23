using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService, VerificationService verificationService) : ControllerBase
{
  private readonly AuthService _authService = authService;
  private readonly VerificationService _verificationService = verificationService;

  [HttpPost("signin")]
  public async Task<IActionResult> SignIn(SignInForm form)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var result = await _authService.SignInAsync(form);
    if (!result.IsSuccess)
      return Unauthorized(result.Message);

    return Ok(result);
  }

  [HttpPost("signup")]
  public async Task<IActionResult> SignUp(SignUpForm form)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    var result = await _authService.RegisterAsync(form);
    if (!result.IsSuccess)
      return BadRequest(result.Message);

    return CreatedAtAction(nameof(SignIn), new { email = form.Email }, result);
  }

  //[FromQuery]

  [HttpPost("verify-email")]
  public async Task<IActionResult> VerifyEmail([FromQuery] string token, [FromQuery] string email)
  {
    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
      return BadRequest("Invalid token or email");

    var result = await _verificationService.VerifyEmailAsync(token, email);
    if (result.Succeeded)
      return Ok(result);

    return BadRequest(result.Errors);
  }
}
