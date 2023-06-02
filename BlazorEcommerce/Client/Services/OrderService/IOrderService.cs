namespace BlazorEcommerce.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> PlaceOrder();
        Task<List<OrderOverviewDto>> GetOrders();
        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}