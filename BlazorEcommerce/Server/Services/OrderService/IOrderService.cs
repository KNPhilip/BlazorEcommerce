namespace BlazorEcommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders();
        Task<ServiceResponse<OrderDetailsDto>> GetOrdersDetailsAsync(int orderId);
    }
}