using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace WebUI.Server.Components.Account.Pages.Manage;

public sealed partial class ExternalLogins
{
    public const string LinkLoginCallbackAction = "LinkLoginCallback";

    private ApplicationUser user = default!;
    private IList<UserLoginInfo>? currentLogins;
    private IList<AuthenticationScheme>? otherLogins;
    private bool showRemoveButton;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromForm]
    private string? LoginProvider { get; set; }

    [SupplyParameterFromForm]
    private string? ProviderKey { get; set; }

    [SupplyParameterFromQuery]
    private string? Action { get; set; }

    protected override async Task OnInitializedAsync()
    {
        user = await UserAccessor.GetRequiredUserAsync(HttpContext);
        currentLogins = await UserManager.GetLoginsAsync(user);
        otherLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => currentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();

        string? passwordHash = null;
        if (UserStore is IUserPasswordStore<ApplicationUser> userPasswordStore)
        {
            passwordHash = await userPasswordStore.GetPasswordHashAsync(user, HttpContext.RequestAborted);
        }

        showRemoveButton = passwordHash is not null || currentLogins.Count > 1;

        if (HttpMethods.IsGet(HttpContext.Request.Method) && Action == LinkLoginCallbackAction)
        {
            await OnGetLinkLoginCallbackAsync();
        }
    }

    private async Task OnSubmitAsync()
    {
        IdentityResult result = await UserManager.RemoveLoginAsync(user, LoginProvider!, ProviderKey!);
        if (!result.Succeeded)
        {
            RedirectManager.RedirectToCurrentPageWithStatus("Error: The external login was not removed.", HttpContext);
        }

        await SignInManager.RefreshSignInAsync(user);
        RedirectManager.RedirectToCurrentPageWithStatus("The external login was removed.", HttpContext);
    }

    private async Task OnGetLinkLoginCallbackAsync()
    {
        string userId = await UserManager.GetUserIdAsync(user);
        ExternalLoginInfo? info = await SignInManager.GetExternalLoginInfoAsync(userId);
        if (info is null)
        {
            RedirectManager.RedirectToCurrentPageWithStatus("Error: Could not load external login info.", HttpContext);
        }

        IdentityResult result = await UserManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            RedirectManager.RedirectToCurrentPageWithStatus("Error: The external login was not added. External logins can only be associated with one account.", HttpContext);
        }

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        RedirectManager.RedirectToCurrentPageWithStatus("The external login was added.", HttpContext);
    }
}
