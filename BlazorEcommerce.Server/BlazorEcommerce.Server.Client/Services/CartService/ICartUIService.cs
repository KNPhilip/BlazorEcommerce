using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Client.Services.CartService
{
    public interface ICartUIService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResponseDto>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResponseDto product);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();
    }
}
