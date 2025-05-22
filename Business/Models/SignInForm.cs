using System.ComponentModel.DataAnnotations;

namespace Business.Models;
public class SignInForm
{
  [Display(Name = "Email", Prompt = "Your email address")]
  [Required(ErrorMessage = "Required")]
  [RegularExpression(@"^(?!\.)(?!.*\.\.)([a-zA-Z0-9._%+-]{1,64})@([a-zA-Z0-9-]{1,63}\.)+[a-zA-Z]{2,}$", ErrorMessage = "Invalid email")]
  public string Email { get; set; } = null!;

  [Display(Name = "Password", Prompt = "Enter your password")]
  [Required(ErrorMessage = "Required")]
  [DataType(DataType.Password)]
  public string Password { get; set; } = null!;
}
