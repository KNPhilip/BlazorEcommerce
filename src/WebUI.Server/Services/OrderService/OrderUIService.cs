using Domain.Dtos;
using Domain.Interfaces;
using Microsoft.Identity.Client;
using WebUI.Server.Services.AuthService;

namespace WebUI.Server.Services.OrderService;

public sealed class OrderUIService(IHttpContextAccessor httpContextAccessor, IOrderService orderService,
    IAuthService authService) : IOrderUIService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IOrderService _orderService = orderService;
    private readonly IAuthService _authService = authService;

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
            string userId = await _authService.GetUserIdAsync();
            await _orderService.PlaceOrder(userId);
            return "https://localhost:7240/order-success/fake";
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
