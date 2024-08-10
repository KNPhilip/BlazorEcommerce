using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using Domain.Models;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ForgotPassword
{
    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    private async Task OnValidSubmitAsync()
    {
        ApplicationUser? user = await UserManager.FindByEmailAsync(Input.Email);
        if (user is null || !(await UserManager.IsEmailConfirmedAsync(user)))
        {
            RedirectManager.RedirectTo("account/forgotpasswordconfirmation");
        }

        string code = await UserManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        string callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("account/resetpassword").AbsoluteUri,
            new Dictionary<string, object?> { ["code"] = code });

        await EmailSender.SendPasswordResetLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

        RedirectManager.RedirectTo("account/forgotpasswordconfirmation");
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
