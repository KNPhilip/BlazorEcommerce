using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Components.Account.Shared;

public sealed partial class ShowRecoveryCodes
{
    [Parameter]
    public string[] RecoveryCodes { get; set; } = [];

    [Parameter]
    public string? StatusMessage { get; set; }
}
