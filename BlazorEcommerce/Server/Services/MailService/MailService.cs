namespace BlazorEcommerce.Server.Services.MailService
{
    /// <summary>
    /// Implementation class of IMailService.
    /// </summary>
    public class MailService : IMailService
    {
        #region Fields
        private readonly MailSettingsDto _mailConfig; 
        #endregion

        #region Constructor
        public MailService(MailSettingsDto mailConfig)
        {
            _mailConfig = mailConfig;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sends an email to the given email, with the title of the given subject.
        /// The email itself simply contains the given HTML body.
        /// </summary>
        /// <param name="ToEmail">Represents the given email address to send the constructed email to.</param>
        /// <param name="Subject">Represents the subject/title of the email.</param>
        /// <param name="HTMLBody">Represents the HTML Body for the email.</param>
        /// <returns>True/False depending on the response, or an appropriate error message in case of failure.</returns>
        public async Task<ServiceResponse<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
        {
            MailMessage message = new();
            SmtpClient smtp = new();

            message.From = new(_mailConfig.FromEmail);
            message.To.Add(new(ToEmail));
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
                return ServiceResponse<bool>.SuccessResponse(true);
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool> { Error = e.Message.ToString() };
            }
        } 
        #endregion
    }
}