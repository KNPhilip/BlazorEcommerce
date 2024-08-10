using Domain.Models;
using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Components.Account.Pages.Manage;

public sealed partial class ResetAuthenticator
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    private async Task OnSubmitAsync()
    {
        ApplicationUser user = await UserAccessor.GetRequiredUserAsync(HttpContext);
        await UserManager.SetTwoFactorEnabledAsync(user, false);
        await UserManager.ResetAuthenticatorKeyAsync(user);
        string userId = await UserManager.GetUserIdAsync(user);
        Logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", userId);

        await SignInManager.RefreshSignInAsync(user);

        RedirectManager.RedirectToWithStatus(
            "account/manage/enableauthenticator",
            "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.",
            HttpContext);
    }
}
