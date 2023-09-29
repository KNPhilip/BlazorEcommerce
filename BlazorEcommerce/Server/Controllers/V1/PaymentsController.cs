namespace BlazorEcommerce.Server.Controllers.V1
{
    /// <summary>
    /// Payment Controller - Contains all endpoints regarding payments & Stripe.
    /// </summary>
    public class PaymentsController : ControllerTemplate
    {
        #region Fields
        /// <summary>
        /// IPaymentService field. Used to access the Payment Services.
        /// </summary>
        private readonly IPaymentService _paymentService;
        #endregion

        #region Constructor
        /// <param name="paymentService">IPaymentService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        #endregion

        #region Endpoints
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
        #endregion
    }
}