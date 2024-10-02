using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using EventHandler.Models.Email;

namespace EventHandler.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(Email request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.to));
            email.Subject = request.subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.body
            };

            using (var smtpClient = new SmtpClient())
            {
                Connect(smtpClient);
                smtpClient.Send(email);
                Disconnect(smtpClient);
            }
        }

        public void Send(EmailBuilder emailBuilder)
        {
            Email email = new EmailDirector(emailBuilder).Construct();
            SendEmail(email);
        }

        private void Connect(SmtpClient smtpClient)
        {
            if (!smtpClient.IsConnected)
            {
                smtpClient.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            }
        }

        private void Disconnect(SmtpClient smtpClient)
        {
            if (smtpClient.IsConnected)
            {
                smtpClient.Disconnect(true);
            }
        }

    }
}
