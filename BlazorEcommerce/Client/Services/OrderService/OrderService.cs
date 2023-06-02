using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(
            HttpClient http,
            AuthenticationStateProvider authStateProvider,
            NavigationManager navigationManager )
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
        }
    }
}