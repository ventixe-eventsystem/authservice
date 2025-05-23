using Azure.Communication.Email;
using Business.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace Infrastructure.Services;
public class EmailService(EmailClient emailClient, IConfiguration config) : IEmailService
{
  private readonly EmailClient _emailClient = emailClient;
  private readonly IConfiguration _config = config;

  public async Task SendVerificationEmailAsync(string email, string token)
  {
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
      throw new ArgumentException("Email and token cannot be null or empty.");

    var subject = "Email Verification";
    var plainTextContent = $@"
      Hello,
      Please verify your email by clicking the link below:
      {_config["App:BaseUrl"]}/verify-email?token={token}?email={email}
      Thank you!";
    var htmlContent = $@"
      <html>
        <body>
          <h1>Email Verification</h1>
          <p>Please verify your email by clicking the link below:</p>
          <a href='{_config["App:BaseUrl"]}/verify-email?token={HttpUtility.UrlEncode(token)}&email={HttpUtility.UrlEncode(email)}'>Verify Email</a>
        </body>
      </html>";

    var emailMessage = new EmailMessage(
      senderAddress: _config["ACS:Sender"],
      recipients: new EmailRecipients([new(email)]),
      content: new EmailContent(subject)
      {
        PlainText = plainTextContent,
        Html = htmlContent
      });

    await _emailClient.SendAsync(Azure.WaitUntil.Completed, emailMessage);
  }
}
