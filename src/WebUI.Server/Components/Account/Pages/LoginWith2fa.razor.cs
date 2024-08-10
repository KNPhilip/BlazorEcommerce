using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class LoginWith2fa
{
    private string? message;
    private ApplicationUser user = default!;

    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    [SupplyParameterFromQuery]
    private bool RememberMe { get; set; }

    protected override async Task OnInitializedAsync()
    {
        user = await SignInManager.GetTwoFactorAuthenticationUserAsync() ??
            throw new InvalidOperationException("Unable to load two-factor authentication user.");
    }

    private async Task OnValidSubmitAsync()
    {
        string authenticatorCode = Input.TwoFactorCode!.Replace(" ", string.Empty).Replace("-", string.Empty);
        SignInResult result = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, RememberMe, Input.RememberMachine);
        string userId = await UserManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            Logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", userId);
            RedirectManager.RedirectTo(ReturnUrl);
        }
        else if (result.IsLockedOut)
        {
            Logger.LogWarning("User with ID '{UserId}' account locked out.", userId);
            RedirectManager.RedirectTo("account/lockout");
        }
        else
        {
            Logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", userId);
            message = "Error: Invalid authenticator code.";
        }
    }

    private sealed class InputModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string? TwoFactorCode { get; set; }

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }
    }
}
