namespace BlazorEcommerce.Server.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailConfig;

        public MailService(MailSettings mailConfig)
        {
            _mailConfig = mailConfig;
        }
    }
}