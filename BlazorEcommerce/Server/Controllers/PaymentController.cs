namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Payment Controller - Contains all endpoints regarding payments & Stripe.
    /// </summary>
    public class PaymentController : ControllerTemplate
    {
        /// <summary>
        /// IPaymentService instance. This accesses the implementation class of the PaymentService through the IoC container.
        /// </summary>
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Creates a new Stripe checkout session.
        /// </summary>
        /// <returns>A string containing the URL for the user to go to next.</returns>
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

        /// <summary>
        /// Endpoint for Stripe to fulfill an order.
        /// </summary>
        /// <returns>True or False depending on the success of the purchase.</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> FulfillOrder() =>
            HandleResult(await _paymentService.FulfillOrder(Request));
    }
}