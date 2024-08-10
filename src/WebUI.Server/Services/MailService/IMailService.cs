using Domain.Dtos;

namespace WebUI.Server.Services.MailService;

public interface IMailService
{
    Task<ResponseDto<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
}
