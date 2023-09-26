namespace BlazorEcommerce.Server.Services.OrderService
{
    /// <summary>
    /// Interface for all things regarding Order Services.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Places an order for the user with the given ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True/False depending on the response.</returns>
        Task<ServiceResponse<bool>> PlaceOrder(int userId);

        /// <summary>
        /// Recieves a list of the details of an order with the given ID.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns>OrderDetailsDto with all of the order details.</returns>
        Task<ServiceResponse<OrderDetailsDto>> GetOrderDetailsAsync(int orderId);

        /// <summary>
        /// Recieves a list of all orders of the currently authenticated user.
        /// </summary>
        /// <returns>A list of OrderOverviewDto.</returns>
        Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders();
    }
}