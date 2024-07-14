using Domain.Dtos;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace WebUI.Server.Services.MailService;

public sealed class MailService(
    IOptions<MailSettingsDto> mailOptions) : IMailService
{
    private readonly MailSettingsDto _mailOptions = mailOptions.Value;

    public async Task<ResponseDto<bool>> SendEmailAsync(string ToEmail, string Subject, string HTMLBody)
    {
        MailMessage message = new();
        SmtpClient smtp = new();

        message.From = new(_mailOptions.FromEmail);
        message.To.Add(new(ToEmail));
        message.Subject = Subject;
        message.IsBodyHtml = true;
        message.Body = HTMLBody;

        smtp.Port = _mailOptions.Port;
        smtp.Host = _mailOptions.Host;
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_mailOptions.Username, _mailOptions.Password);
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
