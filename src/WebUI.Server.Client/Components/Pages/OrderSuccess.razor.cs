﻿using Microsoft.AspNetCore.Components;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class OrderSuccess
{
    [Parameter]
    public string? Status { get; set; }

    protected sealed override async Task OnInitializedAsync()
    {
        await CartUIService.GetCartItemsCount();
    }
}
