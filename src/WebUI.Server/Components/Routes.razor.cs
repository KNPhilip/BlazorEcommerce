namespace WebUI.Server.Components;

public sealed partial class Routes
{
    private void Login()
    {
        NavigationManager.NavigateTo($"login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    }
}
