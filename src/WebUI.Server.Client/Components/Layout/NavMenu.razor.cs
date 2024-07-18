namespace WebUI.Server.Client.Components.Layout;

public sealed partial class NavMenu
{
    private bool showUserMenu = false;

    private string? UserMenuCssClass =>
        showUserMenu ? "show-menu" : null;

    protected sealed override async Task OnInitializedAsync()
    {
        await CategoryUIService.GetCategories();
        CategoryUIService.OnChange += StateHasChanged;
    }

    private void Dispose()
    {
        CategoryUIService.OnChange -= StateHasChanged;
    }

    private async Task Logout()
    {
        await LocalStorage.RemoveItemAsync("authToken");
        await CartUIService.GetCartItemsCount();
        await AuthenticationStateProvider.GetAuthenticationStateAsync();
        NavigationManager.NavigateTo("");
    }

    private void Login()
    {
        NavigationManager.NavigateTo($"login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    }
}
