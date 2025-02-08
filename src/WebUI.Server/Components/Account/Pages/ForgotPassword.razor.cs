using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Domain.Models;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ForgotPassword
{
    private string? statusMessage = string.Empty;

    [SupplyParameterFromForm]
    private ResetPasswordForm Form { get; set; } = new();

    private async Task OnValidSubmitAsync()
    {
        ApplicationUser? user = await UserManager.FindByEmailAsync(Form.Email);
        if (user is null || !(await UserManager.IsEmailConfirmedAsync(user)))
        {
            statusMessage = "We've sent you instructions on resetting your password. Please check your email.";
            return;
        }

        string code = await UserManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        string callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("account/resetpassword").AbsoluteUri,
            new Dictionary<string, object?> { ["code"] = code });

        await EmailSender.SendPasswordResetLinkAsync(user, Form.Email, HtmlEncoder.Default.Encode(callbackUrl));
        statusMessage = $"Go to \"{callbackUrl}\">.";
    }

    private sealed class ResetPasswordForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
