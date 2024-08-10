using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ExternalLogin
{
    public const string LoginCallbackAction = "LoginCallback";

    private string? message;
    private ExternalLoginInfo externalLoginInfo = default!;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    [SupplyParameterFromQuery]
    private string? RemoteError { get; set; }

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery]
    private string? Action { get; set; }

    private string? ProviderDisplayName => externalLoginInfo.ProviderDisplayName;

    protected override async Task OnInitializedAsync()
    {
        if (RemoteError is not null)
        {
            RedirectManager.RedirectToWithStatus("account/login", $"Error from external provider: {RemoteError}", HttpContext);
        }

        ExternalLoginInfo? info = await SignInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            RedirectManager.RedirectToWithStatus("account/login", "Error loading external login information.", HttpContext);
        }

        externalLoginInfo = info;

        if (HttpMethods.IsGet(HttpContext.Request.Method))
        {
            if (Action == LoginCallbackAction)
            {
                await OnLoginCallbackAsync();
                return;
            }

            RedirectManager.RedirectTo("account/login");
        }
    }

    private async Task OnLoginCallbackAsync()
    {
        SignInResult result = await SignInManager.ExternalLoginSignInAsync(
            externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (result.Succeeded)
        {
            Logger.LogInformation(
                "{Name} logged in with {LoginProvider} provider.",
                externalLoginInfo.Principal.Identity?.Name,
                externalLoginInfo.LoginProvider);
            RedirectManager.RedirectTo(ReturnUrl);
        }
        else if (result.IsLockedOut)
        {
            RedirectManager.RedirectTo("account/lockout");
        }

        if (externalLoginInfo.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
        {
            Input.Email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email) ?? "";
        }
    }

    private async Task OnValidSubmitAsync()
    {
        IUserEmailStore<ApplicationUser> emailStore = GetEmailStore();
        ApplicationUser user = CreateUser();

        await UserStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

        IdentityResult result = await UserManager.CreateAsync(user);
        if (result.Succeeded)
        {
            result = await UserManager.AddLoginAsync(user, externalLoginInfo);
            if (result.Succeeded)
            {
                Logger.LogInformation("User created an account using {Name} provider.", externalLoginInfo.LoginProvider);

                string userId = await UserManager.GetUserIdAsync(user);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                string callbackUrl = NavigationManager.GetUriWithQueryParameters(
                    NavigationManager.ToAbsoluteUri("account/confirmemail").AbsoluteUri,
                    new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code });
                await EmailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

                if (UserManager.Options.SignIn.RequireConfirmedAccount)
                {
                    RedirectManager.RedirectTo("account/registerconfirmation", new() { ["email"] = Input.Email });
                }

                await SignInManager.SignInAsync(user, isPersistent: false, externalLoginInfo.LoginProvider);
                RedirectManager.RedirectTo(ReturnUrl);
            }
        }

        message = $"Error: {string.Join(",", result.Errors.Select(error => error.Description))}";
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)UserStore;
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
