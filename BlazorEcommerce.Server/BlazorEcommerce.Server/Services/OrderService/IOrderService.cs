using BlazorEcommerce.Domain.Dtos;

namespace BlazorEcommerce.Server.Services.OrderService;

public interface IOrderService
{
    Task<ResponseDto<bool>> PlaceOrder(int userId);
    Task<ResponseDto<OrderDetailsDto>> GetOrderDetailsAsync(int orderId);
    Task<ResponseDto<List<OrderOverviewDto>>> GetOrders();
}
