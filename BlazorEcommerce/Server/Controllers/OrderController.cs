namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrders()
        {
            var response = await _orderService.GetOrders();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewDto>>>> GetOrderDetails(int orderId)
        {
            var response = await _orderService.GetOrderDetailsAsync(orderId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}