using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Services.AuthService;
using Blazored.LocalStorage;

namespace BlazorEcommerce.Server.Services.CartService;

public sealed class CartUIService(
    ILocalStorageService localStorage,
    IAuthUIService authService,
    ICartService cartService) : ICartUIService
{
    public async Task GetCartItemsCount()
    {
        if (authService.IsUserAuthenticated())
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
}
