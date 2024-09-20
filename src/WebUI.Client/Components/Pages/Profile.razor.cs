using Domain.Dtos;

namespace WebUI.Client.Components.Pages;

public sealed partial class Profile
{
    UserChangePasswordDto request = new();
    string message = string.Empty;

    private async Task ChangePassword()
    {
        bool result = await AuthUIService.ChangePassword(request);
        message = result ? "Password successfully reset" : "Could not reset password, please try again.";
    }
}
