using Microsoft.AspNetCore.Components.Routing;

namespace WebUI.Client.Components.Layout;

public sealed partial class NavMenu
{
    private bool showUserMenu = false;
    private string? currentUrl;

    protected override void OnInitialized()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private string? UserMenuCssClass =>
        showUserMenu ? "show-menu" : null;

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        CategoryUIService.OnChange -= StateHasChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    protected sealed override async Task OnInitializedAsync()
    {
        await CategoryUIService.GetCategories();
        CategoryUIService.OnChange += StateHasChanged;
    }

    private void Login()
    {
        NavigationManager.NavigateTo($"login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    }
}
