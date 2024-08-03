using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ConfirmEmailChange
{
    private string? message;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromQuery]
    private string? UserId { get; set; }

    [SupplyParameterFromQuery]
    private string? Email { get; set; }

    [SupplyParameterFromQuery]
    private string? Code { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (UserId is null || Email is null || Code is null)
        {
            RedirectManager.RedirectToWithStatus(
                "Account/Login", "Error: Invalid email change confirmation link.", HttpContext);
        }

        ApplicationUser? user = await UserManager.FindByIdAsync(UserId);
        if (user is null)
        {
            message = "Unable to find user with Id '{userId}'";
            return;
        }

        string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
        IdentityResult result = await UserManager.ChangeEmailAsync(user, Email, code);
        if (!result.Succeeded)
        {
            message = "Error changing email.";
            return;
        }

        IdentityResult setUserNameResult = await UserManager.SetUserNameAsync(user, Email);
        if (!setUserNameResult.Succeeded)
        {
            message = "Error changing user name.";
            return;
        }

        await SignInManager.RefreshSignInAsync(user);
        message = "Thank you for confirming your email change.";
    }
}
