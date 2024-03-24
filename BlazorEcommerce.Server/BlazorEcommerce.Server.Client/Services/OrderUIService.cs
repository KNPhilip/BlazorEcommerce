using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Server.Client.Services
{
    public sealed class OrderUIService(
        AuthenticationStateProvider authStateProvider,
        HttpClient http) : IOrderUIService
    {
        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            OrderDetailsDto? result = await http
                .GetFromJsonAsync<OrderDetailsDto>($"api/v1/orders/{orderId}");
            return result!;
        }

        public async Task<List<OrderOverviewDto>> GetOrders()
        {
            List<OrderOverviewDto>? result = await http
                .GetFromJsonAsync<List<OrderOverviewDto>>("api/v1/orders");
            return result!;
        }

        public async Task<string> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                HttpResponseMessage result = await http
                    .PostAsync("api/v1/payments/checkout", null);
                return await result.Content.ReadAsStringAsync();
            }
            else return "login";
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await authStateProvider
                .GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
        }
    }
}
