using Stripe;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    /// <summary>
    /// Implementation class of IPaymentService.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// ICartService field. Used to access the Cart Services.
        /// </summary>
        private readonly ICartService _cartService;
        /// <summary>
        /// IAuthService field. Used to access the Auth Services.
        /// </summary>
        private readonly IAuthService _authService;
        /// <summary>
        /// IOrderService field. Used to access the Order Services.
        /// </summary>
        private readonly IOrderService _orderService;
        /// <summary>
        /// IConfiguration field - Used to access the key/value application configuration properties.
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// IHttpContextAccessor field. Used to access the current HTTP context.
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <param name="cartService">ICartService instance to be passed on to the
        /// correct field, containing the correct implementation class through the IoC container.</param>
        /// <param name="authService">IAuthService instance to be passed on to the
        /// correct field, containing the correct implementation class through the IoC container.</param>
        /// <param name="orderService">IOrderService instance to be passed on to the
        /// correct field, containing the correct implementation class through the IoC container.</param>
        /// <param name="configuration">IConfiguration instance to be passed on to the
        /// correct field, containing the correct implementation through the IoC container.</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor instance to be passed on to the
        /// correct field, containing the correct implementation through the IoC container.</param>
        public PaymentService(
            ICartService cartService,
            IAuthService authService,
            IOrderService orderService, 
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration ) 
        {
            _configuration = configuration;
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
            _httpContextAccessor = httpContextAccessor;


            StripeConfiguration.ApiKey = _configuration["StripeAPIKey"];
        }

        /// <summary>
        /// Fake completes the order to simulate a complete purchase.
        /// </summary>
        /// <returns>A string containing the URL to go to next.</returns>
        public async Task<string> FakeOrderCompletion()
        {
            int userId = int.Parse
                (_httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _orderService.PlaceOrder(userId);

            return "https://localhost:7010/order-success/fake";
        }

        /// <summary>
        /// Creates a new Stripe checkout session.
        /// </summary>
        /// <returns>The Stripe session.</returns>
        public async Task<Session> CreateCheckoutSession()
        {
            List<CartProductResponseDto>? products = (await _cartService.GetDbCartItems()).Data;
            List<SessionLineItemOptions> lineItems = new();
            products?.ForEach(product => lineItems.Add(new()
            {
                PriceData = new()
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new()
                    {
                        Name = product.Title,
                        Images = new()
                        {
                            product.ImageUrl
                        }
                    }
                },
                Quantity = product.Quantity
            }));

            SessionCreateOptions options = new()
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new()
                {
                    AllowedCountries = new() { "DK" }
                },
                PaymentMethodTypes = new()
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7010/order-success",
                CancelUrl = "https://localhost:7010/cart"
            };

            SessionService service = new();
            return service.Create(options);
        }

        /// <summary>
        /// Completes the Stripe order.
        /// </summary>
        /// <param name="request">Represents the HttpRequest sent by Stripe.</param>
        /// <returns>True/False depending on the success of the order fulfillment.</returns>
        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            string json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                Event stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        _configuration["StripeWebhookSecret"]
                    );

                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    Session? session = stripeEvent.Data.Object as Session;
                    User? user = await _authService.GetUserByEmail(session?.CustomerEmail!);
                    await _orderService.PlaceOrder(user!.Id);
                }

                return ServiceResponse<bool>.SuccessResponse(true);
            }
            catch(StripeException e)
            {
                return new ServiceResponse<bool> { Error = e.Message };
            }
        }
    }
}