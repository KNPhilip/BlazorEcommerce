using Stripe;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        const string secret = "whsec_8911bf7fc4dd35c31d7360d8cbd61237eca088a2b72cbf187e0aae0c8ef0b750";

        public PaymentService(
            ICartService cartService,
            IAuthService authService,
            IOrderService orderService ) 
        {
            StripeConfiguration.ApiKey = "*Stripe API key here*";

            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
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
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:5100/order-success",
                CancelUrl = "https://localhost:5100/cart"
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
                        secret
                    );

                if(stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = " User ";
                    // -- Place order here --
                }
                return new ServiceResponse<bool>
                {
                    Data = true
                };
            }
            catch(StripeException se)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = se.Message
                };
            }
        }
    }
}