using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace WebUI.Client.Services;

public sealed class CartUIService(
    ILocalStorageService localStorage,
    IAuthUIService authService,
    HttpClient http) : ICartUIService
{
    public event Action? OnChange;

    public async Task AddToCart(CartItem cartItem)
    {
        if (await authService.IsUserAuthenticated())
        {
            await http.PostAsJsonAsync
                ("api/v1/carts/add", cartItem);
        }
        else
        {
            List<CartItem> cart = await localStorage
                .GetItemAsync<List<CartItem>>("cart") ?? [];

            CartItem? sameItem = cart.Find(x => x.ProductId == cartItem.ProductId
            && x.ProductTypeId == cartItem.ProductTypeId);
            if (sameItem is null)
            {
                cart.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await localStorage.SetItemAsync("cart", cart);
        }
        await GetCartItemsCount();
    }

    public async Task GetCartItemsCount()
    {
        if (await authService.IsUserAuthenticated())
        {
            int count = await http
                .GetFromJsonAsync<int>("api/v1/carts/count");

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

        OnChange!.Invoke();
    }

    public async Task<List<CartProductResponseDto>> GetCartProducts()
    {
        if (await authService.IsUserAuthenticated())
        {
            List<CartProductResponseDto>? response = await http
                .GetFromJsonAsync<List<CartProductResponseDto>>
                    ("api/v1/carts");
            return response!;
        }
        else
        {
            List<CartItem>? cartItems = await localStorage
                .GetItemAsync<List<CartItem>>("cart");
            if (cartItems is null)
            {
                return [];
            }
            HttpResponseMessage response = await http
                .PostAsJsonAsync("api/v1/carts/products", cartItems);
            List<CartProductResponseDto>? cartProducts =
                await response.Content.ReadFromJsonAsync
                    <List<CartProductResponseDto>?>();

            return cartProducts!;
        }
    }

    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        if (await authService.IsUserAuthenticated())
        {
            await http.DeleteAsync
                ($"api/v1/carts/{productId}/{productTypeId}");
        }
        else
        {
            List<CartItem>? cart = await localStorage
                .GetItemAsync<List<CartItem>>("cart");
            if (cart is null)
            {
                return;
            }

            CartItem? cartItem = cart.Find(x => x.ProductId == productId
            && x.ProductTypeId == productTypeId);

            if (cartItem is not null)
            {
                cart.Remove(cartItem);
                await localStorage.SetItemAsync("cart", cart);
            }
        }
        await GetCartItemsCount();
    }

    public async Task StoreCartItems(bool emptyLocalCart)
    {
        List<CartItem>? localCart = await localStorage.GetItemAsync<List<CartItem>>("cart");
        if (localCart is null)
        {
            return;
        }

        await http.PostAsJsonAsync("api/v1/carts", localCart);

        if (emptyLocalCart)
        {
            await localStorage.RemoveItemAsync("cart");
        }
    }

    public async Task UpdateQuantity(CartProductResponseDto product)
    {
        if (await authService.IsUserAuthenticated())
        {
            CartItem request = new()
            {
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                ProductTypeId = product.ProductTypeId
            };

            await http.PutAsJsonAsync
                ("api/v1/carts/update-quantity", request);
        }
        else
        {
            List<CartItem>? cart = await localStorage
                .GetItemAsync<List<CartItem>>("cart");
            if (cart is null)
            {
                return;
            }

            CartItem? cartItem = cart
                .Find(x => x.ProductId == product.ProductId
                    && x.ProductTypeId == product.ProductTypeId);

            if (cartItem is not null)
            {
                cartItem.Quantity = product.Quantity;
                await localStorage
                    .SetItemAsync("cart", cart);
            }
        }
    }
}
