using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{
  private readonly AuthService _authService = authService;

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
}
