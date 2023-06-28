namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetCartProducts(List<CartItem> cartItems)
        {
            var response = await _cartService.GetCartProductsAsync(cartItems);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> StoreCartItems(List<CartItem> cartItems)
        {
            var response = await _cartService.StoreCartItemsAsync(cartItems);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
        {
            var response = await _cartService.AddToCart(cartItem);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
        {
            var response = await _cartService.UpdateQuantity(cartItem);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var response = await _cartService.RemoveItemFromCart(productId, productTypeId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
        {
            try
            {
                return Ok(await _cartService.GetCartItemsCountAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetDbCartItems()
        {
            var response = await _cartService.GetDbCartItems();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}