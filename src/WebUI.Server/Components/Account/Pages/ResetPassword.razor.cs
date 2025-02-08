using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ResetPassword
{
    private bool? ResetSuccessful;
    private IEnumerable<IdentityError>? identityErrors;
    private string? Message => identityErrors is null 
        ? null : $"Error: {string.Join(", ", identityErrors.Select(error => error.Description))}";
    
    [SupplyParameterFromForm]
    private ResetPasswordForm Form { get; set; } = new();

    [SupplyParameterFromQuery]
    private string? Code { get; set; }

    protected override void OnInitialized()
    {
        if (Code is null)
        {
            RedirectManager.RedirectTo("account/invalidpasswordreset");
        }
        Form.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
    }

    private async Task OnValidSubmitAsync()
    {
        try
        {
            Domain.Models.ApplicationUser user = await UserManager
                .FindByEmailAsync(Form.Email) ?? throw new Exception();
            IdentityResult resetResult = await UserManager.ResetPasswordAsync(user, Form.Code, Form.Password);

            if (resetResult.Succeeded)
            {
                ResetSuccessful = true;
            }
            else
            {
                identityErrors = resetResult.Errors;
                ResetSuccessful = false;
            }
        }
        catch (Exception)
        {
            IdentityError error = new()
            {
                Description = "Invalid token."
            };
            identityErrors = [error];
            ResetSuccessful = false;
        }
    }

    private sealed class ResetPasswordForm
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
