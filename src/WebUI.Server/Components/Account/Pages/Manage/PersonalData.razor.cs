using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Components.Account.Pages.Manage;

public sealed partial class PersonalData
{
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _ = await UserAccessor.GetRequiredUserAsync(HttpContext);
    }
}
