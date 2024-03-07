namespace BlazorEcommerce.Server.Controllers.V1
{
    /// <summary>
    /// Cart Controller - Contains all endpoints regarding the cart.
    /// </summary>
    public class CartsController : ControllerTemplate
    {
        #region Fields
        /// <summary>
        /// ICartService field. Used to access the Cart Services.
        /// </summary>
        private readonly ICartService _cartService;
        #endregion

        #region Constructor
        /// <param name="cartService">ICartService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Endpoint to recieve the actual products from the cart.
        /// </summary>
        /// <param name="cartItems">Represents the given list of cart items.</param>
        /// <returns>List of products in the cart.</returns>
        [HttpPost("products")]
        public async Task<ActionResult<List<CartProductResponseDto>>> GetCartProducts(List<CartItem> cartItems) =>
            HandleResult(await _cartService.GetCartProductsAsync(cartItems));

        /// <summary>
        /// Stores the given cart items for the user in the database.
        /// </summary>
        /// <param name="cartItems">Represents the given list of cart items.</param>
        /// <returns>A list of the products in the users cart.</returns>
        [HttpPost]
        public async Task<ActionResult<List<CartProductResponseDto>>> StoreCartItems(List<CartItem> cartItems) =>
            HandleResult(await _cartService.StoreCartItemsAsync(cartItems));

        /// <summary>
        /// Endpoint for adding an item to the cart.
        /// </summary>
        /// <param name="cartItem">Represents the given cart item to add to the cart.</param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPost("add")]
        public async Task<ActionResult<bool>> AddToCart(CartItem cartItem) =>
            HandleResult(await _cartService.AddToCart(cartItem));

        /// <summary>
        /// Endpoint to update the quantity of a item in the cart.
        /// </summary>
        /// <param name="cartItem">Represents the given cart item, used to update the quantity.</param>
        /// <returns>True/False depending on the success.</returns>
        [HttpPut("update-quantity")]
        public async Task<ActionResult<bool>> UpdateQuantity(CartItem cartItem) =>
            HandleResult(await _cartService.UpdateQuantity(cartItem));

        /// <summary>
        /// Endpoint that removes a specific item with the given Product ID, with the type of the given Product Type ID from the database.
        /// </summary>
        /// <param name="productId">Represents the given product ID.</param>
        /// <param name="productTypeId">Represents the given product type ID.</param>
        /// <returns>True/False depending on the success.</returns>
        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<bool>> RemoveItemFromCart(int productId, int productTypeId) =>
            HandleResult(await _cartService.RemoveItemFromCart(productId, productTypeId));

        /// <summary>
        /// Endpoint for recieving the number of items in the users cart.
        /// </summary>
        /// <returns>The number of items currently in the cart.</returns>
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCartItemsCount() =>
            HandleResult(await _cartService.GetCartItemsCountAsync());

        /// <summary>
        /// Endpoint for getting all cart items in the database for currently authenticated user.
        /// </summary>
        /// <returns>A list of the products in the cart.</returns>
        [HttpGet]
        public async Task<ActionResult<List<CartProductResponseDto>>> GetDbCartItems() =>
            HandleResult(await _cartService.GetDbCartItems()); 
        #endregion
    }
}