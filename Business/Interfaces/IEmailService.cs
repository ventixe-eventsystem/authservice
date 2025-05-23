namespace Business.Interfaces;
public interface IEmailService
{
  Task SendVerificationEmailAsync(string email, string token);
}
