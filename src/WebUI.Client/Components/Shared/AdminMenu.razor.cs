using System.Security.Claims;

namespace WebUI.Client.Components.Shared;

public sealed partial class AdminMenu
{
    private bool authorized = false;

    protected override async Task OnInitializedAsync()
    {
        string? role = (await AuthStateProvider.GetAuthenticationStateAsync())
            .User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;

        if (role.Contains("Admin"))
        {
            authorized = true;
        }
    }
}
