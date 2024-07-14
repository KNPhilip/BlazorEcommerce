using Domain.Dtos;
using Domain.Interfaces;

namespace WebUI.Server.Services.OrderService;

public sealed class OrderUIService(
    IOrderService orderService) : IOrderUIService
{
    public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
    {
        ResponseDto<OrderDetailsDto> result = await orderService
            .GetOrderDetailsAsync(orderId);
        return result.Data!;
    }

    public async Task<List<OrderOverviewDto>> GetOrders()
    {
        ResponseDto<List<OrderOverviewDto>> result = 
            await orderService.GetOrders();
        return result.Data!;
    }

    public async Task<string> PlaceOrder()
    {
        return "https://localhost:7010/order-success/fake";
    }
}
