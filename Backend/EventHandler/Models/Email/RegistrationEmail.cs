using EventHandler.Dto;

namespace EventHandler.Models.Email
{
    public class RegistrationEmail
    {
        private Email email;
        private UserDto user;
        public RegistrationEmail(UserDto user)
        {
            this.user = user;
            this.email = new Email();
        }
        public Email Build()
        {
            return email;
        }

        public void BuildBody()
        {
            email.body = $"<h3>Welcome {user.FirstName} {user.LastName},</h3>" +
                            "<p>Thank you for registering with us. We are excited to have you on board.</p>" +
                            "<p>Best Regards,<br>" +
                            "NFTFY Team</p>";
        }

        public void BuildSubject()
        {
            email.subject = "Welcome to Nftfy – Registration Successful!";
        }

        public void BuildTo()
        {
            email.to = user.Email;
        }
    }
}
