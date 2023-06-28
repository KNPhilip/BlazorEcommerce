namespace BlazorEcommerce.Server.Services.MailService
{
    public interface IMailService
    {
        Task<ServiceResponse<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
    }
}