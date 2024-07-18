using Domain.Models;
using MudBlazor;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class ForgotPassword
{
    private readonly User user = new();

    private async void SendMail()
    {
        string? response = await AuthUIService.CreateResetToken(user);
        if (response is not null)
        {
            snackMessage(response,
                Severity.Success, Defaults.Classes.Position.BottomLeft);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            snackMessage("Could not send email, please try again.", Severity.Error, Defaults.Classes.Position.BottomLeft);
        }
    }

    private void snackMessage(string message, MudBlazor.Severity type, string position)
    {
        Action<SnackbarOptions> config = (SnackbarOptions options) =>
        {
            options.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
        };

        Snackbar.Clear();
        Snackbar.Configuration.PositionClass = position;
        Snackbar.Add(message, type);
    }
}
