using Domain.Dtos;
using Microsoft.AspNetCore.WebUtilities;

namespace WebUI.Client.Components.Pages;

public sealed partial class Login
{
    private readonly UserLoginDto user = new();
    private string errorMessage = string.Empty;
    private string returnUrl = string.Empty;

    protected override void OnInitialized()
    {
        Uri uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
        {
            returnUrl = url!;
        }
    }

    private async Task HandleLogin()
    {
        string? result = await AuthUIService.Login(user);
        if (result is not null)
        {
            errorMessage = string.Empty;

            await LocalStorage.SetItemAsync("authToken", result);
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            await CartUIService.StoreCartItems(true);
            await CartUIService.GetCartItemsCount();
            NavigationManager.NavigateTo(returnUrl);
        }
        else
        {
            errorMessage = "Could not log in, please try again.";
        }
    }
}
