using BlazorEcommerce.Domain.Dtos;

namespace BlazorEcommerce.Server.Services.MailService
{
    public interface IMailService
    {
        Task<ResponseDto<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
    }
}
