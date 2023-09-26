using Stripe;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    /// <summary>
    /// Implementation class of IPaymentService.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// ICartService instance. This accesses the implementation class of the CartService through the IoC container.
        /// </summary>
        private readonly ICartService _cartService;
        /// <summary>
        /// IAuthService instance. This accesses the implementation class of the AuthService through the IoC container.
        /// </summary>
        private readonly IAuthService _authService;
        /// <summary>
        /// IOrderService instance. This accesses the implementation class of the OrderService through the IoC container.
        /// </summary>
        private readonly IOrderService _orderService;
        /// <summary>
        /// IConfiguration instance - Represents a set of key/value application configuration properties.
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// Instance of the HTTP Context.
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

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