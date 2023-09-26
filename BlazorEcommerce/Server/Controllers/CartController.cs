namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Cart Controller - Contains all endpoints regarding the cart.
    /// </summary>
    public class CartController : ControllerTemplate
    {
        /// <summary>
        /// ICartService instance. This accesses the implementation class of the CartService through the IoC container.
        /// </summary>
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Endpoint to recieve the actual products from the cart.
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns>List of products in the cart.</returns>
        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetCartProducts(List<CartItem> cartItems) =>
            HandleResult(await _cartService.GetCartProductsAsync(cartItems));

        /// <summary>
        /// Stores the given cart items for the user in the database.
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns>A list of the products in the users cart.</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> StoreCartItems(List<CartItem> cartItems) =>
            HandleResult(await _cartService.StoreCartItemsAsync(cartItems));

        /// <summary>
        /// Endpoint for adding an item to the cart.
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem) =>
            HandleResult(await _cartService.AddToCart(cartItem));

        /// <summary>
        /// Endpoint to update the quantity of a item in the cart.
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem) =>
            HandleResult(await _cartService.UpdateQuantity(cartItem));

        /// <summary>
        /// Endpoint that removes a specific item with the given Product ID, with the type of the given Product Type ID from the database.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productTypeId"></param>
        /// <returns>True/False depending on the success.</returns>
        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId) =>
            HandleResult(await _cartService.RemoveItemFromCart(productId, productTypeId));

        /// <summary>
        /// Endpoint for recieving the number of items in the users cart.
        /// </summary>
        /// <returns>The number of items currently in the cart.</returns>
        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount() =>
            HandleResult(await _cartService.GetCartItemsCountAsync());

        /// <summary>
        /// Endpoint for getting all cart items in the database for currently authenticated user.
        /// </summary>
        /// <returns>A list of the products in the cart.</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetDbCartItems() =>
            HandleResult(await _cartService.GetDbCartItems());
    }
}