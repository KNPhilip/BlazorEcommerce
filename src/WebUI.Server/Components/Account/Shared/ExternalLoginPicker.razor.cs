using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Components.Account.Shared;

public sealed partial class ExternalLoginPicker
{
    private AuthenticationScheme[] externalLogins = [];

    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        externalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).ToArray();
    }
}
