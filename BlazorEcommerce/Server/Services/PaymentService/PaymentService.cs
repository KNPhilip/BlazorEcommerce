using Stripe;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
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

        public async Task<string> FakeOrderCompletion()
        {
            int userId = int.Parse
                (_httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _orderService.PlaceOrder(userId);

            return "https://localhost:7010/order-success/fake";
        }

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