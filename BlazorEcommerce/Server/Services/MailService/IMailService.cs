namespace BlazorEcommerce.Server.Services.MailService
{
    /// <summary>
    /// Interface for all things regarding Mail Services.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends an email to the given email, with the title of the given subject.
        /// The email itself simply contains the given HTML body.
        /// </summary>
        /// <param name="ToEmail"></param>
        /// <param name="Subject"></param>
        /// <param name="HTMLBody"></param>
        /// <returns>True/False depending on the response.</returns>
        Task<ServiceResponse<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
    }
}