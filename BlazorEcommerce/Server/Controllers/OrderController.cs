﻿namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Order Controller - Contains all endpoints regarding orders.
    /// </summary>
    public class OrderController : ControllerTemplate
    {
        /// <summary>
        /// IOrderService field. Used to access the Order Services.
        /// </summary>
        private readonly IOrderService _orderService;

        /// <param name="orderService">IOrderService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Endpoint to recieve all orders from the database.
        /// </summary>
        /// <returns>A list of all the orders, or an error message in case of failure.</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrders() =>
            HandleResult(await _orderService.GetOrders());

        /// <summary>
        /// Endpoint to get the order details of a specific order with the given ID.
        /// </summary>
        /// <param name="orderId">Represents the ID of the order you want to recieve details from.</param>
        /// <returns>"OrderOverviewDto" with all needed details, or error message in case something went wrong.</returns>
        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrderDetails(int orderId) =>
            HandleResult(await _orderService.GetOrderDetailsAsync(orderId));
    }
}