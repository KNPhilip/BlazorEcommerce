using Domain.Dtos;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class Register
{
    UserRegisterDto user = new();
    string resultMessage = string.Empty;
    string resultMessageCssClass = string.Empty;

    private async Task HandleRegistration()
    {
        bool result = await AuthUIService.Register(user);
        resultMessage = result ? "Successfully registered" : "Could not register, please try again.";
        resultMessageCssClass = result ? "text-success" : "text-danger";
    }
}
