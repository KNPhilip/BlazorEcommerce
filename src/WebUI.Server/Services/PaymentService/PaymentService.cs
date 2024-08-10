using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Services.AuthService;
using WebUI.Server.Services.CartService;
using WebUI.Server.Services.OrderService;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace WebUI.Server.Services.PaymentService;

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
        IConfiguration configuration)
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
        string userId = await _authService.GetUserIdAsync();

        await _orderService.PlaceOrder(userId);

        return "https://localhost:7240/order-success/fake";
    }

    public async Task<Session> CreateCheckoutSession()
    {
        List<CartProductResponseDto>? products = (await _cartService.GetDbCartItems()).Data;
        List<SessionLineItemOptions> lineItems = [];
        products?.ForEach(product => lineItems.Add(new()
        {
            PriceData = new()
            {
                UnitAmountDecimal = product.Price * 100,
                Currency = "usd",
                ProductData = new()
                {
                    Name = product.Title,
                    Images =
                    [
                        product.ImageUrl
                    ]
                }
            },
            Quantity = product.Quantity
        }));

        SessionCreateOptions options = new()
        {
            CustomerEmail = _authService.GetUserEmail(),
            ShippingAddressCollection = new()
            {
                AllowedCountries = ["DK"]
            },
            PaymentMethodTypes =
            [
                "card"
            ],
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "https://localhost:7240/order-success",
            CancelUrl = "https://localhost:7240/cart"
        };

        SessionService service = new();
        return service.Create(options);
    }

    public async Task<ResponseDto<bool>> FulfillOrder(HttpRequest request)
    {
        string json = await new StreamReader(request.Body).ReadToEndAsync();
        try
        {
            Event stripeEvent = EventUtility.ConstructEvent(
                json,
                request.Headers["Stripe-Signature"],
                _configuration["StripeWebhookSecret"]
            );

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                Session? session = stripeEvent.Data.Object as Session;
                ApplicationUser? user = await _authService.GetUserByEmail(session!.CustomerEmail);
                await _orderService.PlaceOrder(user!.Id);
            }

            return ResponseDto<bool>.SuccessResponse(true);
        }
        catch (StripeException e)
        {
            return ResponseDto<bool>.ErrorResponse(e.Message);
        }
    }
}
