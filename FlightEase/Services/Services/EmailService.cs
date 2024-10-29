using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("FlightEase System", "minhnvse172756@fpt.edu.vn\r\n"));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                // Connect to the SMTP server
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Authenticate with your email and App Password
                await client.AuthenticateAsync("minhnvse172756@fpt.edu.vn",
                    "gxlf jvio clpb gsto"); // Use your App Password here

                // Send the email
                await client.SendAsync(emailMessage);

                // Disconnect from the SMTP server
                await client.DisconnectAsync(true);
            }
        }
    }
}