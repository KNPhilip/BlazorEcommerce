using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ResetPassword
{
    private IEnumerable<IdentityError>? identityErrors;

    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    [SupplyParameterFromQuery]
    private string? Code { get; set; }

    private string? Message => identityErrors is null 
        ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";

    protected override void OnInitialized()
    {
        if (Code is null)
        {
            RedirectManager.RedirectTo("account/invalidpasswordreset");
        }

        Input.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
    }

    private async Task OnValidSubmitAsync()
    {
        if (await UserManager.FindByEmailAsync(Input.Email) is null)
        {
            RedirectManager.RedirectTo("account/resetpasswordconfirmation");
        }

        if ((await UserManager.ResetPasswordAsync(await UserManager
            .FindByEmailAsync(Input.Email) ?? throw new Exception("Could not reset password of that user."),
                Input.Code, Input.Password)).Succeeded)
        {
            RedirectManager.RedirectTo("account/resetpasswordconfirmation");
        }

        identityErrors = (await UserManager.ResetPasswordAsync(
            await UserManager.FindByEmailAsync(Input.Email) ?? throw new Exception("Could not reset password of that user."),
                Input.Code, Input.Password)).Errors;
    }

    private sealed class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        public string Code { get; set; } = "";
    }
}
