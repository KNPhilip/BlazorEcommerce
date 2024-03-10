using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Client.Services.AuthService;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services.CartService
{
    public class CartUIService : ICartUIService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly IAuthUIService _authService;

        public CartUIService(
            ILocalStorageService localStorage, 
            HttpClient http, 
            IAuthUIService authService )
        {
            _localStorage = localStorage;
            _http = http;
            _authService = authService;
        }

        public event Action? OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await _authService.IsUserAuthenticated())
                await _http.PostAsJsonAsync("api/v1/carts/add", cartItem);
            else
            {
                List<CartItem> cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? [];

                CartItem? sameItem = cart.Find(x => x.ProductId == cartItem.ProductId
                && x.ProductTypeId == cartItem.ProductTypeId);
                if (sameItem is null)
                    cart.Add(cartItem);
                else
                    sameItem.Quantity += cartItem.Quantity;

                await _localStorage.SetItemAsync("cart", cart);
            }
            await GetCartItemsCount();
        }

        public async Task GetCartItemsCount()
        {
            if (await _authService.IsUserAuthenticated())
            {
                int result = await _http.GetFromJsonAsync<int>("api/v1/carts/count");
                int count = result;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                List<CartItem>? cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cart is not null ? cart.Count : 0);
            }

            OnChange!.Invoke();
        }

        public async Task<List<CartProductResponseDto>> GetCartProducts()
        {
            if (await _authService.IsUserAuthenticated())
            {
                List<CartProductResponseDto>? response = await _http.GetFromJsonAsync<List<CartProductResponseDto>>("api/v1/carts");
                return response!;
            }
            else
            {
                List<CartItem>? cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems is null)
                    return new List<CartProductResponseDto>();
                var response = await _http.PostAsJsonAsync("api/v1/carts/products", cartItems);
                List<CartProductResponseDto>? cartProducts =
                    await response.Content.ReadFromJsonAsync<List<CartProductResponseDto>?>();

                return cartProducts!;
            }
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            if (await _authService.IsUserAuthenticated())
                await _http.DeleteAsync($"api/v1/carts/{productId}/{productTypeId}");
            else
            {
                List<CartItem>? cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart is null)
                    return;

                CartItem? cartItem = cart.Find(x => x.ProductId == productId
                && x.ProductTypeId == productTypeId);

                if (cartItem is not null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
            await GetCartItemsCount();
        }

        public async Task StoreCartItems(bool emptyLocalCart)
        {
            List<CartItem>? localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart is null)
                return;

            await _http.PostAsJsonAsync("api/v1/carts", localCart);

            if (emptyLocalCart)
                await _localStorage.RemoveItemAsync("cart");
        }

        public async Task UpdateQuantity(CartProductResponseDto product)
        {
            if (await _authService.IsUserAuthenticated())
            {
                CartItem request = new()
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    ProductTypeId = product.ProductTypeId
                };

                await _http.PutAsJsonAsync("api/v1/carts/update-quantity", request);
            }
            else
            {
                List<CartItem>? cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart is null)
                    return;

                CartItem? cartItem = cart.Find(x => x.ProductId == product.ProductId
                && x.ProductTypeId == product.ProductTypeId);

                if (cartItem is not null)
                {
                    cartItem.Quantity = product.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }
        }
    }
}
