namespace BlazorEcommerce.Server.Controllers
{
    public class CartController : ControllerTemplate
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetCartProducts(List<CartItem> cartItems) =>
            HandleResult(await _cartService.GetCartProductsAsync(cartItems));

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> StoreCartItems(List<CartItem> cartItems) =>
            HandleResult(await _cartService.StoreCartItemsAsync(cartItems));

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem) =>
            HandleResult(await _cartService.AddToCart(cartItem));

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem) =>
            HandleResult(await _cartService.UpdateQuantity(cartItem));

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId) =>
            HandleResult(await _cartService.RemoveItemFromCart(productId, productTypeId));

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount() =>
            HandleResult(await _cartService.GetCartItemsCountAsync());

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetDbCartItems() =>
            HandleResult(await _cartService.GetDbCartItems());
    }
}