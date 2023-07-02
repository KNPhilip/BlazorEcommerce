namespace BlazorEcommerce.Client.Services.MailService
{
    public interface IMailService
    {
        Task<ServiceResponse<bool>> SendEmail(SendMailDto request);
    }
}