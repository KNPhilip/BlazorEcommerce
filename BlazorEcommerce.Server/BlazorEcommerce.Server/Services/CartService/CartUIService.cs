using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Interfaces;
using BlazorEcommerce.Domain.Models;
using Blazored.LocalStorage;

namespace BlazorEcommerce.Server.Services.CartService;

public sealed class CartUIService(
    ILocalStorageService localStorage,
    IAuthUIService authService,
    ICartService cartService) : ICartUIService
{
    public event Action? OnChange;

    public async Task GetCartItemsCount()
    {
        if (await authService.IsUserAuthenticated())
        {
            ResponseDto<int> result = await cartService.GetCartItemsCountAsync();
            int count = result.Data;

            await localStorage.SetItemAsync
                ("cartItemsCount", count);
        }
        else
        {
            List<CartItem>? cart = await localStorage
                .GetItemAsync<List<CartItem>>("cart");

            await localStorage.SetItemAsync
                ("cartItemsCount", cart is not null ? cart.Count : 0);
        }
    }

    public Task AddToCart(CartItem cartItem) =>
        throw new NotImplementedException();

    public Task<List<CartProductResponseDto>> GetCartProducts() =>
        throw new NotImplementedException();

    public Task RemoveProductFromCart(int productId, int productTypeId) =>
        throw new NotImplementedException();

    public Task StoreCartItems(bool emptyLocalCart) =>
        throw new NotImplementedException();

    public Task UpdateQuantity(CartProductResponseDto product) =>
        throw new NotImplementedException();
}
