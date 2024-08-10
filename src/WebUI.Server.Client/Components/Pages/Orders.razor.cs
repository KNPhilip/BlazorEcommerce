﻿using Domain.Dtos;

namespace WebUI.Server.Client.Components.Pages;

public sealed partial class Orders
{
    private List<OrderOverviewDto>? orders = null;

    protected override async Task OnInitializedAsync()
    {
        orders = await OrderUIService.GetOrders();
    }
}
