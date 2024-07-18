using Domain.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MudBlazor;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class ResetPassword
{
    private PasswordResetDto resetDto = new();
    private StringValues token = String.Empty;
    private StringValues mail = String.Empty;
    private string? userToken { get; set; }
    private string? userMail { get; set; }
    private bool validUserToken = false;
    private string message = "No issues found..";

    protected override async Task OnInitializedAsync()
    {
        Uri uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("token", out token))
        {
            userToken = Convert.ToString(token);
        }
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("user", out mail))
        {
            userMail = Convert.ToString(mail);
        }

        if (userMail is not null && userToken is not null)
        {
            TokenValidateDto dto = new()
            {
                UserEmail = userMail,
                ResetToken = userToken
            };

            bool response = await AuthUIService.ValidateResetPasswordToken(dto);
            validUserToken = response;
            message = response ? message : "Something went wrong, please try again.";
        }
    }

    private async Task HandleReset()
    {
        resetDto.ResetToken = userToken!;
        resetDto.UserEmail = userMail!;
        bool response = await AuthUIService.ResetPassword(resetDto);
        if (!response)
        {
            snackMessage("Failed to reset password", Severity.Error, Defaults.Classes.Position.BottomRight);
        }
        else
        {
            snackMessage("Password was successfully reset", Severity.Success, Defaults.Classes.Position.BottomRight);
            int Count = 3;
            Timer tmr = new(new TimerCallback(_ =>
            {
                if (Count > 0)
                {
                    Count--;
                }
                if (Count == 0)
                {
                    SnackBar.Clear();
                    NavigationManager.NavigateTo("/login");
                }
            }), null, 1000, 1000);
        }
    }

    private void snackMessage(string message, Severity type, string position)
    {
        SnackBar.Clear();
        SnackBar.Configuration.PositionClass = position;
        SnackBar.Add(message, type);
    }
}
