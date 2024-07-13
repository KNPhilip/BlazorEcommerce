using BlazorEcommerce.Domain.Dtos;
using System.Net;
using System.Net.Mail;

namespace BlazorEcommerce.Server.Services.MailService;

public sealed class MailService(
    MailSettingsDto mailConfig) : IMailService
{
    private readonly MailSettingsDto _mailConfig = mailConfig;

    public async Task<ResponseDto<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
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
            return ResponseDto<bool>.SuccessResponse(true);
        }
        catch (Exception e)
        {
            return ResponseDto<bool>.ErrorResponse(e.Message.ToString());
        }
    }
}
