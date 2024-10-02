namespace EventHandler.Models.Email
{
    public class EmailDirector
    {
        EmailBuilder emailBuilder;
        public EmailDirector(EmailBuilder emailBuilder)
        {
            this.emailBuilder = emailBuilder;
        }
        public Email Construct()
        {
            emailBuilder.BuildTo();
            emailBuilder.BuildSubject();
            emailBuilder.BuildBody();
            return emailBuilder.Build();
        }
    }
}
