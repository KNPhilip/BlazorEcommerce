namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        public PaymentService(
            ICartService cartService,
            IAuthService authService,
            IOrderService orderService ) 
        {
            _cartService = cartService;
            _authService = authService;
            _orderService = orderService;
        }
    }
}