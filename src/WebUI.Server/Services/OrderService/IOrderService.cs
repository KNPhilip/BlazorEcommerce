using Domain.Dtos;

namespace WebUI.Server.Services.OrderService;

public interface IOrderService
{
    Task<ResponseDto<bool>> PlaceOrder(int userId);
    Task<ResponseDto<OrderDetailsDto>> GetOrderDetailsAsync(int orderId);
    Task<ResponseDto<List<OrderOverviewDto>>> GetOrders();
}
