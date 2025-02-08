using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class Login
{
    private string? statusMessage = string.Empty;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromForm]
    private LoginForm? Form { get; set; }

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Form ??= new();
        if (HttpMethods.IsGet(HttpContext.Request.Method))
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
    }

    public async Task LoginUserAsync()
    {
        SignInResult result = await SignInManager.PasswordSignInAsync(Form!.Email, Form.Password, Form.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            Logger.LogInformation("User logged in.");
            RedirectManager.RedirectTo(ReturnUrl);
        }
        else if (result.RequiresTwoFactor)
        {
            RedirectManager.RedirectTo("account/loginwith2fa",
                new() { ["returnUrl"] = ReturnUrl, ["rememberMe"] = Form.RememberMe });
        }
        else if (result.IsLockedOut)
        {
            Logger.LogWarning("User account locked out.");
            RedirectManager.RedirectTo("account/lockout");
        }
        else
        {
            statusMessage = "Invalid login attempt.";
        }
    }

    //public async Task ResendEmailConfirmationAsync(string email)
    //{
    //    if (new EmailAddressAttribute().IsValid(email))
    //    {
    //        isError = true;
    //        statusMessage = "You need to enter a valid email address to confirm your email.";
    //        return;
    //    }

    //    ApplicationUser? user = await UserManager.FindByEmailAsync(email);
    //    if (user is null)
    //    {
    //        isError = false;
    //        statusMessage = "Verification email sent. Please check your email.";
    //        return;
    //    }

    //    string userId = await UserManager.GetUserIdAsync(user);
    //    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
    //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    //    string callbackUrl = NavigationManager.GetUriWithQueryParameters(
    //    NavigationManager.ToAbsoluteUri("account/confirmemail").AbsoluteUri,
    //        new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code });
    //    await EmailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(callbackUrl));

    //    isError = false;
    //    statusMessage = "Verification email sent. Please check your email.";
    //}

    private sealed class LoginForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
