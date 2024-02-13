using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace PMT.Services.Email;

public class MyEmailSender : IMyEmailSender
{
  public MyEmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
  {
    Options = optionsAccessor.Value;
  }

  //Set with Secret Manager
  public AuthMessageSenderOptions Options { get; }

  public Task SendEmailAsync(string toEmail, string subject, string message)
  {
    var client = new SmtpClient("smtp-mail.outlook.com                     test breaking this service", 587)
    {
      EnableSsl = true,
      UseDefaultCredentials = false,
      Credentials = new NetworkCredential(Str.AppEmail, Options.AppEmailPassword)
    };

    var msg = new MailMessage(Str.AppEmail, to: toEmail, subject, message)
    {
      IsBodyHtml = true
    };

    return client.SendMailAsync(msg);
  }
}
