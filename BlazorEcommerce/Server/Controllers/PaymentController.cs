namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("checkout"), Authorize]
        public async Task<ActionResult<string>> CreateCheckoutSession()
        {
            try
            {
                var session = await _paymentService.CreateCheckoutSession();
                return Ok(session.Url);
            }
            catch
            {
                var url = await _paymentService.FakeOrderCompletion();
                return Ok(url);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder()
        {
            var response = await _paymentService.FulfillOrder(Request);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }
    }
}