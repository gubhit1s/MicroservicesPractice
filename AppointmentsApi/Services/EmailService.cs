using MimeKit;
using MailKit.Net.Smtp;
namespace AppointmentsApi.Services;

public class EmailService : IEmailService
{
  public async Task SendEmailAsync(string to, string subject, string body)
  {
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Healthcare System", "no-reply@healthcare.com"));
    message.To.Add(new MailboxAddress("Patient", to));
    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };

    using var client = new SmtpClient();
    await client.ConnectAsync("smtp-relay.brevo.com", 587, false);
    await client.AuthenticateAsync("letuananhmessi00@gmail.com", "bskx8ksJKCFVd4W");
    await client.SendAsync(message);
    await client.DisconnectAsync(true);
  }
}

public interface IEmailService
{
  Task SendEmailAsync(string to, string subject, string body);
}