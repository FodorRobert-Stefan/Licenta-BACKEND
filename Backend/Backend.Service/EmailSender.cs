using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Backend.DataAbstraction;

namespace Backend.Services
{
  public class EmailSender : IEmailSender
  {
    private readonly IEmailConfig _configuration;

    public EmailSender(IEmailConfig configuration)
    {
      _configuration = configuration;
    }

    public void SendEmail(string to, string subject, string htmlBody)
    {
      var fromAddress = _configuration.From;
      var smtpHost = _configuration.Host;
      var smtpPort = _configuration.Port;
      var username = _configuration.Username;
      var password = _configuration.Password;

      var message = new MailMessage
      {
        From = new MailAddress(fromAddress),
        Subject = subject,
        Body = htmlBody,
        IsBodyHtml = true
      };

      message.To.Add(to);

      using var client = new SmtpClient(smtpHost, smtpPort)
      {
        Credentials = new NetworkCredential(username, password),
        EnableSsl = true
      };

      client.Send(message);
    }
  }
}
