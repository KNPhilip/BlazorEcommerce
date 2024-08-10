namespace WebUI.Server.Components.Account.Shared;

public sealed partial class ManageNavMenu
{
    private bool hasExternalLogins;

    protected override async Task OnInitializedAsync()
    {
        hasExternalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).Any();
    }
}
