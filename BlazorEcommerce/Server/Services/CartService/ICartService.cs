namespace BlazorEcommerce.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemsCountAsync();
        Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartItems(int? userId = null);
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
        Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId);
    }
}