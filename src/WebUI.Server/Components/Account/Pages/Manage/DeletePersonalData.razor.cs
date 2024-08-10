using Domain.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Server.Components.Account.Pages.Manage;

public sealed partial class DeletePersonalData
{
    private string? message;
    private ApplicationUser user = default!;
    private bool requirePassword;

    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromForm]
    private InputModel Input { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Input ??= new();
        user = await UserAccessor.GetRequiredUserAsync(HttpContext);
        requirePassword = await UserManager.HasPasswordAsync(user);
    }

    private async Task OnValidSubmitAsync()
    {
        if (requirePassword && !await UserManager.CheckPasswordAsync(user, Input.Password))
        {
            message = "Error: Incorrect password.";
            return;
        }

        IdentityResult result = await UserManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Unexpected error occurred deleting user.");
        }

        await SignInManager.SignOutAsync();

        string userId = await UserManager.GetUserIdAsync(user);
        Logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        RedirectManager.RedirectToCurrentPage();
    }

    private sealed class InputModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
