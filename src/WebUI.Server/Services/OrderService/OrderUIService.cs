using Domain.Dtos;
using Domain.Interfaces;
using Microsoft.Identity.Client;

namespace WebUI.Server.Services.OrderService;

public sealed class OrderUIService(IOrderService orderService, IHttpContextAccessor httpContextAccessor
    ) : IOrderUIService
{
    private readonly IOrderService _orderService = orderService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
    {
        ResponseDto<OrderDetailsDto> result = await _orderService
            .GetOrderDetailsAsync(orderId);
        return result.Data!;
    }

    public async Task<List<OrderOverviewDto>> GetOrders()
    {
        ResponseDto<List<OrderOverviewDto>> result = 
            await _orderService.GetOrders();
        return result.Data!;
    }

    public async Task<string> PlaceOrder()
    {
        await Task.Run(() => {});
        if (IsUserAuthenticated())
        {
            return "https://localhost:7010/order-success/fake";
        }
        else
        {
            return "login";
        }
    }

    private bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext!.User.Identity!.IsAuthenticated;
    }
}
