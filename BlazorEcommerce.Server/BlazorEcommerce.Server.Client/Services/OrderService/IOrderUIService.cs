using BlazorEcommerce.Domain.Dtos;

namespace BlazorEcommerce.Server.Client.Services.OrderService
{
    public interface IOrderUIService
    {
        Task<string> PlaceOrder();
        Task<List<OrderOverviewDto>> GetOrders();
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
