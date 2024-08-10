using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace WebUI.Server.Components.Account.Pages;

public sealed partial class ConfirmEmail
{
    private string? statusMessage;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromQuery]
    private string? UserId { get; set; }

    [SupplyParameterFromQuery]
    private string? Code { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (UserId is null || Code is null)
        {
            RedirectManager.RedirectTo("");
        }

        ApplicationUser? user = await UserManager.FindByIdAsync(UserId);
        if (user is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            statusMessage = $"Error loading user with ID {UserId}";
        }
        else
        {
            string code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Code));
            IdentityResult result = await UserManager.ConfirmEmailAsync(user, code);
            statusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        }
    }
}
