using Stripe.Checkout;

namespace BlazorEcommerce.Server.Controllers
{
    public class PaymentController : ControllerTemplate
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
                Session session = await _paymentService.CreateCheckoutSession();
                return Ok(session.Url);
            }
            catch
            {
                string url = await _paymentService.FakeOrderCompletion();
                return Ok(url);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder() =>
            HandleResult(await _paymentService.FulfillOrder(Request));
    }
}