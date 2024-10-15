using Microsoft.Extensions.Options;
using System.Net;
using MailKit.Net.Smtp;
using MailKit;
using MailKit.Security;
using MimeKit;

namespace Proyecto_Bb_2.Servicios
{
    public class EmailService
    {
        private readonly EmailConfig _emailConfig;
        public EmailService(IOptions<EmailConfig> _email)
        {
            _emailConfig = _email.Value;
        }
        public async Task EnviarEmail(string asunto, string cuerpo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailConfig.FromEmail));
            email.To.Add(MailboxAddress.Parse("juan.tito.dev@gmail.com"));
            email.Subject = asunto;
            email.Body = new TextPart("html") { Text = cuerpo };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_emailConfig.Host, _emailConfig.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailConfig.UserName, _emailConfig.PassWord);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
