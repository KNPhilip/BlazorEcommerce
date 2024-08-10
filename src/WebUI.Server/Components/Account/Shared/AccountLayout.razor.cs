using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Components.Account.Shared;

public sealed partial class AccountLayout
{
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    protected override void OnParametersSet()
    {
        if (HttpContext is null)
        {
            NavigationManager.Refresh(forceReload: true);
        }
    }
}
