namespace BlazorEcommerce.Server.Controllers
{
    public class OrderController : ControllerTemplate
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrders() =>
            HandleResult(await _orderService.GetOrders());

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrderDetails(int orderId) =>
            HandleResult(await _orderService.GetOrderDetailsAsync(orderId));
    }
}