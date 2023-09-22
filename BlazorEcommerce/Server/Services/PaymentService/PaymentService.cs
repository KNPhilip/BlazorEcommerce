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
            var userId = int.Parse
                (_httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier));

            await _orderService.PlaceOrder(userId);

            return "https://localhost:7010/order-success/fake";
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartItems()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string>
                        {
                            product.ImageUrl
                        }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "DK" }
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7010/order-success",
                CancelUrl = "https://localhost:7010/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        _configuration["StripeWebhookSecret"]
                    );

                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);
                    await _orderService.PlaceOrder(user.Id);
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