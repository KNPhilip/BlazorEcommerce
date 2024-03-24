namespace BlazorEcommerce.Domain.Interfaces
{
    public interface IOrderUIService
    {
        Task<string> PlaceOrder();
        Task<List<OrderOverviewDto>> GetOrders();
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}
