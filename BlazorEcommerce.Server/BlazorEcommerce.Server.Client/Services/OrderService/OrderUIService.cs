using BlazorEcommerce.Domain.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services.OrderService
{
    public class OrderUIService : IOrderUIService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderUIService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider,
            NavigationManager navigationManager )
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            OrderDetailsDto? result = await _http
                .GetFromJsonAsync<OrderDetailsDto>($"api/v1/orders/{orderId}");
            return result!;
        }

        public async Task<List<OrderOverviewDto>> GetOrders()
        {
            List<OrderOverviewDto>? result = await _http
                .GetFromJsonAsync<List<OrderOverviewDto>>("api/v1/orders");
            return result!;
        }

        public async Task<string> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.PostAsync("api/v1/payments/checkout", null);
                return await result.Content.ReadAsStringAsync();
            }
            else return "login";
        }

        private async Task<bool> IsUserAuthenticated() =>
            (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
    }
}
