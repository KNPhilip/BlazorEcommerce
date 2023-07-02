namespace BlazorEcommerce.Server.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailConfig;

        public MailService(MailSettings mailConfig)
        {
            _mailConfig = mailConfig;
        }

        public async Task<ServiceResponse<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
        {
            MailMessage message = new();
            SmtpClient smtp = new();

            message.From = new MailAddress(_mailConfig.FromEmail);
            message.To.Add(new MailAddress(ToEmail));
            message.Subject = Subject;
            message.IsBodyHtml = true;
            message.Body = HTMLBody;

            smtp.Port = _mailConfig.Port;
            smtp.Host = _mailConfig.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_mailConfig.Username, _mailConfig.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                await smtp.SendMailAsync(message);

                var response = new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Mail was send successfully."
                };

                return response;
            }
            catch (Exception e)
            {
                var response = new ServiceResponse<bool>
                {
                    Success = false,
                    Message = e.Message.ToString()
                };

                return response;
            }
        }
    }
}