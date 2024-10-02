using EventHandler.Models.Email;

namespace EventHandler.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(Email request);
        void Send(EmailBuilder emailBuilder);
        
    }
}
